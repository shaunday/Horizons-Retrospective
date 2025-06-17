using Asp.Versioning;
using AutoMapper;
using HsR.Common;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Web.Services.Models.Journal;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using HsR.Web.API.Services;

namespace HsR.Web.API.Controllers.Journal
{
    [Route("hsr-api/v{version:apiVersion}/journal/trades")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TradesJournalController : JournalControllerBase
    {
        private readonly IConfigurationService _config;

        public TradesJournalController(
            IJournalRepositoryWrapper journalAccess,
            ILogger<JournalControllerBase> logger,
            IMapper mapper,
            ITradesCacheService cacheService,
            IConfigurationService config) : base(journalAccess, logger, mapper, cacheService)
        {
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeCompositeModel>>> GetAllTrades(int pageNumber = 1, int pageSize = 0)
        {
            pageSize = pageSize == 0 ? _config.Pagination.DefaultPageSize : ValidatePageSize(pageSize);
            
            // Try to get from cache first
            var cachedTrades = await _cacheService.GetCachedTrades(pageNumber, pageSize);
            if (cachedTrades != null)
            {
                return Ok(cachedTrades);
            }

            // If not in cache, get from database
            var (tradeEntities, paginationMetadata) = await _journalAccess.Journal.GetAllTradeCompositesAsync(pageNumber, pageSize);
            var models = _mapper.Map<IEnumerable<TradeCompositeModel>>(tradeEntities);
            SetPaginationHeader(paginationMetadata);

            return Ok(models);
        }

        [HttpGet("byFilter")]
        public async Task<ActionResult<IEnumerable<TradeCompositeModel>>> 
                                                    GetFilteredTrades(TradesFilterModel filter, int pageNumber = 1, int pageSize = 0)
        {
            pageSize = pageSize == 0 ? _config.Pagination.DefaultPageSize : ValidatePageSize(pageSize);

            var (filteredTradesEntities, paginationMetadata) = await _journalAccess.Journal.GetFilteredTradesAsync(filter, pageNumber, pageSize);

            SetPaginationHeader(paginationMetadata);

            IEnumerable<TradeCompositeModel> resAsModels = _mapper.Map<IEnumerable<TradeCompositeModel>>(filteredTradesEntities);

            return Ok(resAsModels);
        }

        [HttpPost]
        public async Task<ActionResult<TradeCompositeModel>> AddTrade()
        {
            var positionComposite = await _journalAccess.Journal.AddTradeCompositeAsync();
            TradeCompositeModel resAsModel = _mapper.Map<TradeCompositeModel>(positionComposite);

            // Invalidate cache when a new trade is added and start reload
            _cacheService.InvalidateCache();

            return Ok(resAsModel);
        }

        #region Helper methods

        private int ValidatePageSize(int pageSize)
        {
            return pageSize > _config.Pagination.MaxPageSize ? _config.Pagination.MaxPageSize : pageSize;
        }

        private void SetPaginationHeader(PaginationMetadata paginationMetadata)
        {
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(paginationMetadata);
        }

        #endregion
    }
}
