using AssetsFlowWeb.Services.Models.Journal;
using AutoMapper;
using HsR.Common.Services.Caching;
using HsR.Journal.DataContext;
using HsR.Web.API.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HsR.Web.API.Services
{
    public interface ITradesCacheService : ICacheService<Guid, IEnumerable<TradeCompositeModel>>
    {
        int GetCachedTotalCount(Guid userId);
        Task<IEnumerable<TradeCompositeModel>?> GetCachedTrades(Guid userId, int pageNumber, int pageSize);
    }

    public class TradesCacheService : CacheServiceBase<Guid, IEnumerable<TradeCompositeModel>>, ITradesCacheService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly ILogger<TradesCacheService> _logger;
        private readonly IConfigurationService _config;

        protected override string CacheKeyPrefix => "trades_page";
        private string TotalCountKey(Guid userId) => $"trades_total_count_{userId}";

        public TradesCacheService(
            IMemoryCache memoryCache,
            IServiceProvider serviceProvider,
            IMapper mapper,
            ILogger<TradesCacheService> logger,
            IConfigurationService config)
            : base(
                  memoryCache,
                  logger,
                  TimeSpan.FromMinutes(config.Cache.CacheDurationMinutes),
                  config.Cache.MaxConcurrentUsers)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _logger = logger;
            _config = config;
        }

        protected override async Task<IEnumerable<TradeCompositeModel>?> LoadFromSourceAsync(Guid userId, string? subKey, CancellationToken token)
        {
            var allTrades = new List<TradeCompositeModel>();
            int pageNumber = 1;
            int pageSize = _config.Pagination.DefaultPageSize;

            if (subKey != null && int.TryParse(subKey.Split('_')[0], out var pn))
                pageNumber = pn;

            using var scope = _serviceProvider.CreateScope();
            var journal = scope.ServiceProvider.GetRequiredService<IJournalRepositoryWrapper>();

            // total count
            var (_, totalCount) = await journal.Journal.GetAllTradeCompositesAsync(userId, 1, 1);
            SetTotalCount(userId, totalCount);

            // preload pages
            for (int page = pageNumber; page <= _config.Cache.PreloadPageCount; page++)
            {
                token.ThrowIfCancellationRequested();

                var (tradeEntities, _) = await journal.Journal.GetAllTradeCompositesAsync(userId, page, pageSize);
                if (tradeEntities == null || !tradeEntities.Any()) break;

                var models = _mapper.Map<IEnumerable<TradeCompositeModel>>(tradeEntities);
                allTrades.AddRange(models);

                Set(userId, models, $"{page}_{pageSize}");
            }

            return allTrades;
        }

        private void SetTotalCount(Guid userId, int totalCount)
        {
            // store total count in cache as a fake TradeCompositeModel (so it fits IEnumerable<T>)
            base.Set(userId, new List<TradeCompositeModel> { new TradeCompositeModel { Id = totalCount } }, TotalCountKey(userId));
        }

        public int GetCachedTotalCount(Guid userId)
        {
            var cached = base.GetAsync(userId, TotalCountKey(userId), null, TimeSpan.FromSeconds(_config.Cache.LoadWaitTimeoutSeconds)).Result;
            return cached?.FirstOrDefault()?.Id ?? 0;
        }

        public async Task<IEnumerable<TradeCompositeModel>?> GetCachedTrades(Guid userId, int pageNumber, int pageSize)
        {
            string key = $"{pageNumber}_{pageSize}";
            return await base.GetAsync(userId, key, null, TimeSpan.FromSeconds(_config.Cache.LoadWaitTimeoutSeconds));
        }
    }
}