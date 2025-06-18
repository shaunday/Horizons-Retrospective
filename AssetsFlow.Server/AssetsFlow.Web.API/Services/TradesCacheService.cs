using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Web.API.Configuration;
using HsR.Web.Services.Models.Journal;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System.Threading;

namespace HsR.Web.API.Services
{
    public interface ITradesCacheService
    {
        Task<IEnumerable<TradeCompositeModel>?> GetCachedTrades(int pageNumber, int pageSize);
        void InvalidateAndReload();
        int GetCachedTotalCount();
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

        private Task? _loadTask;
        private static readonly object _invalidationLock = new();
        private static CancellationTokenSource? _cacheTokenSource;
        private CancellationTokenSource? _loadCts;
        private static readonly SemaphoreSlim _loadSemaphore = new(1, 1);
        private static readonly TimeSpan LoadWaitTimeout = TimeSpan.FromSeconds(2);

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

            _cacheTokenSource = new CancellationTokenSource();
            _loadCts = new CancellationTokenSource();
        }

        // Public getter for cached trades
        public async Task<IEnumerable<TradeCompositeModel>?> GetCachedTrades(int pageNumber, int pageSize)
        {
            try
            {
                string cacheKey = $"{CacheKeyPrefix}{pageNumber}_{pageSize}";
                var cachedData = _cache.Get<IEnumerable<TradeCompositeModel>>(cacheKey);

                if (cachedData != null)
                    return cachedData;

                if (_loadTask != null && !_loadTask.IsCompleted)
                {
                    try
                    {
                        using var cts = new CancellationTokenSource(LoadWaitTimeout);
                        await _loadTask.WaitAsync(cts.Token);

                        return _cache.Get<IEnumerable<TradeCompositeModel>>(cacheKey);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogDebug("Cache load wait timed out for page {PageNumber}", pageNumber);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving cached trades for page {PageNumber}", pageNumber);
                return null;
            }
        }

        // Public total count getter
        public int GetCachedTotalCount()
        {
            return _cache.TryGetValue(TotalCountKey, out int totalCount) ? totalCount : 0;
        }

        // Public invalidate and reload, the only way to trigger cache reload
        public void InvalidateAndReload()
        {
            lock (_invalidationLock)
            {
                try
                {
                    _logger.LogDebug("Starting cache invalidation");

                    // Cancel ongoing load
                    _loadCts?.Cancel();
                    _loadCts?.Dispose();
                    _loadCts = new CancellationTokenSource();

                    // Invalidate cache entries
                    _cacheTokenSource?.Cancel();
                    _cacheTokenSource?.Dispose();
                    _cacheTokenSource = new CancellationTokenSource();

                    _logger.LogDebug("Cache invalidation triggered");

                    // Fire & forget reload task
                    _loadTask = Task.Run(() => LoadCacheAsync(_loadCts.Token));
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during cache invalidation");
                }
            }
        }

        // Private cache loading logic
        private async Task LoadCacheAsync(CancellationToken cancellationToken)
        {
            await _loadSemaphore.WaitAsync(cancellationToken);
            try
            {
                _logger.LogDebug("Starting cache load");

                using var scope = _serviceProvider.CreateScope();
                var journalAccess = scope.ServiceProvider.GetRequiredService<IJournalRepositoryWrapper>();

                for (int page = 1; page <= _config.Cache.PreloadPageCount; page++)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await LoadPageIntoCache(journalAccess, page, _config.Pagination.DefaultPageSize, cancellationToken);
                }

                _logger.LogDebug("Cache load completed");
            }
            catch (OperationCanceledException)
            {
                _logger.LogDebug("Cache load cancelled");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during cache load");
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
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken)
        {
            string cacheKey = $"{CacheKeyPrefix}{pageNumber}_{pageSize}";

            var (tradeEntities, totalCount) = await journalAccess.Journal.GetAllTradeCompositesAsync(pageNumber, pageSize);
            var models = _mapper.Map<IEnumerable<TradeCompositeModel>>(tradeEntities);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(_config.Cache.CacheDurationMinutes))
                .SetPriority(CacheItemPriority.Normal)
                .AddExpirationToken(new CancellationChangeToken(_cacheTokenSource!.Token));

            _cache.Set(cacheKey, models, cacheOptions);
            _cache.Set(TotalCountKey, totalCount, cacheOptions);

            _logger.LogDebug("Loaded page {PageNumber} into cache", pageNumber);
        }
    }
}