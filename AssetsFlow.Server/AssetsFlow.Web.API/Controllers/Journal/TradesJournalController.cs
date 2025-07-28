using Asp.Versioning;
using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Web.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;
using System.Text.Json;
using Serilog;
using HsR.UserService.Client.Interfaces;
using HsR.UserService.Protos;
using Microsoft.AspNetCore.Authorization;
using AssetsFlowWeb.Services.Models.Journal;

namespace HsR.Web.API.Controllers.Journal
{
    [Route("hsr-api/v{version:apiVersion}/journal/trades")]
    [ApiVersion("1.0")]
    [ApiController]
    [Authorize]
    public class TradesJournalController : JournalControllerBase
    {
        private readonly IConfigurationService _config;
        private readonly IUserServiceClient _userServiceClient;

        public TradesJournalController(
            IJournalRepositoryWrapper journalAccess,
            ILogger<JournalControllerBase> logger,
            IMapper mapper,
            ITradesCacheService cacheService,
            IConfigurationService config,
            IUserServiceClient userServiceClient) : base(journalAccess, logger, mapper, cacheService)
        {
            _config = config;
            _userServiceClient = userServiceClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeCompositeModel>>> GetAllTrades(int pageNumber = 1, int pageSize = 0)
        {
            var userId = GetUserIdFromClaims();
            pageSize = ValidatePageSize(pageSize);
            if (pageNumber <= 0) pageNumber = 1;

            IEnumerable<TradeCompositeModel>? paginatedTradeDTOs;
            int totalTradesCount;

            paginatedTradeDTOs = await _cacheService.GetCachedTrades(userId, pageNumber, pageSize);
            if (paginatedTradeDTOs != null)
            {
                totalTradesCount = _cacheService.GetCachedTotalCount(userId);
            }
            else  // If not in cache, get from database
            {
                var (tradeEntities, totalCount) = await _journalAccess.Journal.GetAllTradeCompositesAsync(userId, pageNumber, pageSize);
                paginatedTradeDTOs = _mapper.Map<IEnumerable<TradeCompositeModel>>(tradeEntities);
                totalTradesCount = totalCount;
            }

            SetPaginationHeaders(totalTradesCount, pageSize, pageNumber);

            return Ok(paginatedTradeDTOs);
        }

        [HttpPost]
        public async Task<ActionResult<TradeCompositeModel>> AddTrade()
        {
            var userId = GetUserIdFromClaims();
            var positionComposite = await _journalAccess.Journal.AddTradeCompositeAsync(userId);
            TradeCompositeModel resAsModel = _mapper.Map<TradeCompositeModel>(positionComposite);

            _cacheService.InvalidateAndReload(userId);

            return Ok(resAsModel);
        }

        [HttpGet("{tradeId}")]
        public async Task<ActionResult<TradeCompositeModel>> GetTradeById(int tradeId)
        {
            var userId = GetUserIdFromClaims();
            try
            {
                var trade = await _journalAccess.Journal.GetTradeCompositeByIdAsync(tradeId);
                if (trade == null)
                    return NotFound();
                var tradeModel = _mapper.Map<TradeCompositeModel>(trade);
                return Ok(tradeModel);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error getting trade by Id: {TradeId}", tradeId);
                var (status, msg) = ExceptionMappingService.MapToStatusCode(ex);
                return StatusCode(status, msg);
            }
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

        private void SetPaginationHeaders(int totalCount, int pageSize, int pageNumber)
        {
            Response.Headers["X-Total-Count"] = totalCount.ToString();
            Response.Headers["X-Page-Number"] = pageNumber.ToString();
            Response.Headers["X-Page-Size"] = pageSize.ToString();
        }
        #endregion
    }
}
