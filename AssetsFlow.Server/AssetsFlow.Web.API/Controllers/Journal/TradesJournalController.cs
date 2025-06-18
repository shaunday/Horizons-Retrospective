using Asp.Versioning;
using AutoMapper;
using HsR.Common;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Web.API.Services;
using HsR.Web.Services.Models.Journal;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Text.Json;

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
            pageSize = ValidatePageSize(pageSize);

            IEnumerable<TradeCompositeModel>? paginatedTradeDTOs;
            int totalTradesCount;

            // Try to get from cache first
            paginatedTradeDTOs = await _cacheService.GetCachedTrades(pageNumber, pageSize);
            if (paginatedTradeDTOs != null)
            {
                totalTradesCount = _cacheService.GetCachedTotalCount();
            }
            else  // If not in cache, get from database
            {              
                var (tradeEntities, totalCount) = await _journalAccess.Journal.GetAllTradeCompositesAsync(pageNumber, pageSize);
                paginatedTradeDTOs = _mapper.Map<IEnumerable<TradeCompositeModel>>(tradeEntities);
                totalTradesCount = totalCount;
            }

            SetPaginationHeader(totalTradesCount, pageSize, pageNumber);

            return Ok(paginatedTradeDTOs);
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
            if (pageSize <= 0)
                pageSize = _config.Pagination.DefaultPageSize;
            if (pageSize > _config.Pagination.MaxPageSize)
                pageSize = _config.Pagination.MaxPageSize;
            return pageSize;
        }

        private void SetPaginationHeader(int totalCount, int pageSize, int pageNumber)
        {
            var paginationMetadata = new PaginationMetadata(totalCount, pageSize, pageNumber);
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(paginationMetadata);
        }

        #endregion
    }
}
