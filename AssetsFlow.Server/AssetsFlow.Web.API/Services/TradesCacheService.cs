using HsR.Journal.DataContext;
using HsR.Web.Services.Models.Journal;
using Microsoft.Extensions.Caching.Memory;
using AutoMapper;
using Microsoft.Extensions.Logging;
using HsR.Web.API.Configuration;
using Microsoft.Extensions.Primitives;

namespace HsR.Web.API.Services
{
    public interface ITradesCacheService
    {
        Task<IEnumerable<TradeCompositeModel>?> GetCachedTrades(int pageNumber, int pageSize);
        Task LoadCache();
        void InvalidateCache();
        int GetCachedTotalCount();
    }

    public class TradesCacheService : ITradesCacheService
    {
        private readonly IMemoryCache _cache;
        private readonly IJournalRepositoryWrapper _journalAccess;
        private readonly IMapper _mapper;
        private readonly ILogger<TradesCacheService> _logger;
        private readonly IConfigurationService _config;
        private const string CacheKeyPrefix = "trades_page_";
        private const string TotalCountKey = "trades_total_count";
        private Task? _loadTask;
        private static readonly TimeSpan LoadWaitTimeout = TimeSpan.FromSeconds(2);
        private static readonly object _invalidationLock = new();
        private static CancellationTokenSource? _cacheTokenSource;
        private static CancellationTokenSource? _loadCts;

        public TradesCacheService(
            IMemoryCache cache,
            IJournalRepositoryWrapper journalAccess,
            IMapper mapper,
            ILogger<TradesCacheService> logger,
            IConfigurationService config)
        {
            _cache = cache;
            _journalAccess = journalAccess;
            _mapper = mapper;
            _logger = logger;
            _config = config;
            _cacheTokenSource = new CancellationTokenSource();
            _loadCts = new CancellationTokenSource();
        }

        public async Task<IEnumerable<TradeCompositeModel>?> GetCachedTrades(int pageNumber, int pageSize)
        {
            try
            {
                string cacheKey = $"{CacheKeyPrefix}{pageNumber}_{pageSize}";
                var cachedData = _cache.Get<IEnumerable<TradeCompositeModel>>(cacheKey);
                
                if (cachedData != null)
                {
                    return cachedData;
                }

                // If cache miss and load is in progress, wait for it briefly
                if (_loadTask != null && !_loadTask.IsCompleted)
                {
                    try
                    {
                        using var cts = new CancellationTokenSource(LoadWaitTimeout);
                        await _loadTask.WaitAsync(cts.Token);
                        
                        // Check cache again after load completes
                        return _cache.Get<IEnumerable<TradeCompositeModel>>(cacheKey);
                    }
                    catch (OperationCanceledException)
                    {
                        _logger.LogDebug("Load wait timed out for page {PageNumber}", pageNumber);
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting cached trades for page {PageNumber}", pageNumber);
                return null;
            }
        }

        public async Task LoadCache()
        {
            try
            {
                _logger.LogDebug("Starting cache load");
                // Load first few pages with default page size
                for (int page = 1; page <= _config.Cache.PreloadPageCount; page++)
                {
                    _loadCts?.Token.ThrowIfCancellationRequested();
                    await LoadPageIntoCache(page, _config.Pagination.DefaultPageSize);
                }
                _logger.LogDebug("Cache load completed");
            }
            catch (OperationCanceledException)
            {
                _logger.LogDebug("Cache load was cancelled");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during cache load");
            }
            finally
            {
                _loadTask = null;
            }
        }

        private async Task LoadPageIntoCache(int pageNumber, int pageSize)
        {
            string cacheKey = $"{CacheKeyPrefix}{pageNumber}_{pageSize}";
            
            var (tradeEntities, totalCount) = await _journalAccess.Journal.GetAllTradeCompositesAsync(pageNumber, pageSize);
            var models = _mapper.Map<IEnumerable<TradeCompositeModel>>(tradeEntities);

            var cacheOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(_config.Cache.CacheDurationMinutes))
                .SetPriority(CacheItemPriority.Normal)
                .AddExpirationToken(new CancellationChangeToken(_cacheTokenSource!.Token));

            _cache.Set(cacheKey, models, cacheOptions);
            _cache.Set(TotalCountKey, totalCount, cacheOptions);
            _logger.LogDebug("Loaded page {PageNumber} into cache", pageNumber);
        }

        public void InvalidateCache()
        {
            lock (_invalidationLock)
            {
                try
                {
                    _logger.LogDebug("Starting cache invalidation");
                    
                    // Cancel any existing load task
                    _loadCts?.Cancel();
                    _loadCts?.Dispose();
                    _loadCts = new CancellationTokenSource();
                    
                    // Cancel the existing token source to invalidate all cache entries
                    _cacheTokenSource?.Cancel();
                    _cacheTokenSource?.Dispose();
                    
                    // Create a new token source for future cache entries
                    _cacheTokenSource = new CancellationTokenSource();

                    _logger.LogDebug("Cache invalidation triggered");

                    // Start async reload in background (fire and forget)
                    _loadTask = LoadCache();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during cache invalidation");
                }
            }
        }

        public int GetCachedTotalCount()
        {
            return _cache.TryGetValue(TotalCountKey, out int totalCount) ? totalCount : 0;
        }
    }
} 