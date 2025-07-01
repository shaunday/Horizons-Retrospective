using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Web.API.Configuration;
using HsR.Web.Services.Models.Journal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Threading;

namespace HsR.Web.API.Services
{
    public interface ITradesCacheService
    {
        Task<IEnumerable<TradeCompositeModel>?> GetCachedTrades(Guid userId, int pageNumber, int pageSize);
        void InvalidateAndReload(Guid userId);
        int GetCachedTotalCount(Guid userId);
        void CleanupInactiveUsers(TimeSpan inactivityThreshold);
    }

    public class TradesCacheService : ITradesCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly ILogger<TradesCacheService> _logger;
        private readonly IConfigurationService _config;

        private const string CacheKeyPrefix = "trades_page_";
        private const string TotalCountKey = "trades_total_count";

        private readonly Dictionary<Guid, Task?> _loadTasks = new();
        private readonly Dictionary<Guid, CancellationTokenSource> _loadCts = new();
        private readonly Dictionary<Guid, CancellationTokenSource> _cacheTokenSources = new();
        private static SemaphoreSlim _loadSemaphore = new(1, 1);
        private static TimeSpan LoadWaitTimeout = TimeSpan.FromSeconds(10); // Default, will be overwritten in ctor

        public TradesCacheService(
            IMemoryCache cache,
            IServiceProvider serviceProvider,
            IMapper mapper,
            ILogger<TradesCacheService> logger,
            IConfigurationService config)
        {
            _cache = cache;
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _logger = logger;
            _config = config;
            _loadSemaphore = new SemaphoreSlim(1, 1);
            LoadWaitTimeout = TimeSpan.FromSeconds(_config.Cache.LoadWaitTimeoutSeconds);
        }

        private MemoryCacheEntryOptions CreateCacheEntryOptions(Guid userId)
        {
            return new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(_config.Cache.CacheDurationMinutes))
                .SetPriority(CacheItemPriority.Normal)
                .SetSize(1)
                .AddExpirationToken(new CancellationChangeToken(_cacheTokenSources[userId].Token))
                .RegisterPostEvictionCallback((key, value, reason, state) =>
                {
                    _logger.LogDebug("Cache entry {Key} evicted due to {Reason}", key, reason);
                });
        }

        public static string GetCacheKey(Guid userId, int pageNumber, int pageSize)
        {
            return $"{CacheKeyPrefix}{userId}_{pageNumber}_{pageSize}";
        }

        // Public getter for cached trades
        public async Task<IEnumerable<TradeCompositeModel>?> GetCachedTrades(Guid userId, int pageNumber, int pageSize)
        {
            try
            {
                string cacheKey = GetCacheKey(userId, pageNumber, pageSize);
                var cachedData = _cache.Get<IEnumerable<TradeCompositeModel>>(cacheKey);

                if (cachedData != null)
                    return cachedData;

                if (_loadTasks.TryGetValue(userId, out var loadTask) && loadTask != null && !loadTask.IsCompleted)
                {
                    try
                    {
                        using var cts = new CancellationTokenSource(LoadWaitTimeout);
                        await loadTask.WaitAsync(cts.Token);

                        return _cache.Get<IEnumerable<TradeCompositeModel>>(cacheKey);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogDebug("Cache load wait timed out for user {UserId}, page {PageNumber}", userId, pageNumber);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cached trades for user {UserId}, page {PageNumber}", userId, pageNumber);
                return null;
            }
        }

        // Public total count getter
        public int GetCachedTotalCount(Guid userId)
        {
            string totalCountKey = $"{TotalCountKey}_{userId}";
            return _cache.TryGetValue(totalCountKey, out int totalCount) ? totalCount : 0;
        }

        // Public invalidate and reload, the only way to trigger cache reload
        public void InvalidateAndReload(Guid userId)
        {
            lock (_loadSemaphore)
            {
                // Enforce dictionary size limit
                if (_loadTasks.Count >= _config.Cache.MaxConcurrentUsers || _loadCts.Count >= _config.Cache.MaxConcurrentUsers || _cacheTokenSources.Count >= _config.Cache.MaxConcurrentUsers)
                {
                    CleanupInactiveUsers(TimeSpan.FromHours(_config.Cache.CleanupInactiveUsersThresholdHours));
                }
                try
                {
                    _logger.LogDebug("Starting cache invalidation for user {UserId}", userId);

                    // Cancel ongoing load for this user
                    if (_loadCts.TryGetValue(userId, out var loadCts))
                    {
                        loadCts.Cancel();
                        loadCts.Dispose();
                    }
                    _loadCts[userId] = new CancellationTokenSource();

                    // Invalidate cache entries for this user
                    if (_cacheTokenSources.TryGetValue(userId, out var cacheTokenSource))
                    {
                        cacheTokenSource.Cancel();
                        cacheTokenSource.Dispose();
                    }
                    _cacheTokenSources[userId] = new CancellationTokenSource();

                    _logger.LogDebug("Cache invalidation triggered for user {UserId}", userId);

                    // Fire & forget reload task
                    _loadTasks[userId] = Task.Run(() => LoadCacheAsync(userId, _loadCts[userId].Token));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during cache invalidation for user {UserId}", userId);
                }
            }
        }

        // Private cache loading logic
        private async Task LoadCacheAsync(Guid userId, CancellationToken cancellationToken)
        {
            await _loadSemaphore.WaitAsync(cancellationToken);
            try
            {
                _logger.LogDebug("Starting cache load for user {UserId}", userId);

                using var scope = _serviceProvider.CreateScope();
                var journalAccess = scope.ServiceProvider.GetRequiredService<IJournalRepositoryWrapper>();

                // Get total count once for the user
                var (_, totalCount) = await journalAccess.Journal.GetAllTradeCompositesAsync(userId, 1, 1);
                string totalCountKey = $"{TotalCountKey}_{userId}";
                var cacheOptions = CreateCacheEntryOptions(userId);
                _cache.Set(totalCountKey, totalCount, cacheOptions);

                for (int page = 1; page <= _config.Cache.PreloadPageCount; page++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await LoadPageIntoCache(journalAccess, userId, page, _config.Pagination.DefaultPageSize, cancellationToken);
                }

                _logger.LogDebug("Cache load completed for user {UserId}", userId);
            }
            catch (OperationCanceledException)
            {
                _logger.LogDebug("Cache load cancelled for user {UserId}", userId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during cache load for user {UserId}", userId);
                throw;
            }
            finally
            {
                _loadSemaphore.Release();
            }
        }

        // Private single page loader
        private async Task LoadPageIntoCache(
            IJournalRepositoryWrapper journalAccess,
            Guid userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            string cacheKey = GetCacheKey(userId, pageNumber, pageSize);

            var (tradeEntities, _) = await journalAccess.Journal.GetAllTradeCompositesAsync(userId, pageNumber, pageSize);
            var models = _mapper.Map<IEnumerable<TradeCompositeModel>>(tradeEntities);

            var cacheOptions = CreateCacheEntryOptions(userId);

            _cache.Set(cacheKey, models, cacheOptions);

            _logger.LogDebug("Loaded page {PageNumber} into cache for user {UserId}", pageNumber, userId);
        }

        // Cleanup method for inactive users
        public void CleanupInactiveUsers(TimeSpan inactivityThreshold)
        {
            var cutoffTime = DateTime.UtcNow.Subtract(inactivityThreshold);
            
            // Clean up completed tasks
            var completedTasks = _loadTasks.Where(kvp => kvp.Value?.IsCompleted == true).ToList();
            foreach (var kvp in completedTasks)
            {
                _loadTasks.Remove(kvp.Key);
            }

            // Clean up unused cancellation token sources
            var unusedCts = _loadCts.Where(kvp => !_loadTasks.ContainsKey(kvp.Key)).ToList();
            foreach (var kvp in unusedCts)
            {
                kvp.Value.Dispose();
                _loadCts.Remove(kvp.Key);
            }

            var unusedCacheTokens = _cacheTokenSources.Where(kvp => !_loadTasks.ContainsKey(kvp.Key)).ToList();
            foreach (var kvp in unusedCacheTokens)
            {
                kvp.Value.Dispose();
                _cacheTokenSources.Remove(kvp.Key);
            }

            _logger.LogDebug("Cleaned up {CompletedTasks} completed tasks and {UnusedCts} unused cancellation sources", 
                completedTasks.Count, unusedCts.Count + unusedCacheTokens.Count);
        }
    }

    // Background service for cache cleanup
    public class CacheCleanupService : BackgroundService
    {
        private readonly ITradesCacheService _cacheService;
        private readonly ILogger<CacheCleanupService> _logger;
        private readonly TimeSpan _cleanupInterval;
        private readonly TimeSpan _inactivityThreshold;

        public CacheCleanupService(ITradesCacheService cacheService, ILogger<CacheCleanupService> logger, IConfigurationService configService)
        {
            _cacheService = cacheService;
            _logger = logger;
            _cleanupInterval = TimeSpan.FromMinutes(configService.Cache.CleanupIntervalMinutes);
            _inactivityThreshold = TimeSpan.FromHours(configService.Cache.CleanupInactiveUsersThresholdHours);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _cacheService.CleanupInactiveUsers(_inactivityThreshold);
                    _logger.LogDebug("Cache cleanup completed");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during cache cleanup");
                }

                await Task.Delay(_cleanupInterval, stoppingToken);
            }
        }
    }
}