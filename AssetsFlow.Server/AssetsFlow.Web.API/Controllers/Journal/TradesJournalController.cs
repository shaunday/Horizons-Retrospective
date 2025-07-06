using Asp.Versioning;
using HsR.Web.Services.Models.Journal;
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

namespace HsR.Web.API.Controllers.Journal
{
    [Route("hsr-api/v{version:apiVersion}/journal/trades")]
    [ApiVersion("1.0")]
    [ApiController]
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
        public async Task<ActionResult<IEnumerable<TradeCompositeModel>>> GetAllTrades(Guid userId, int pageNumber = 1, int pageSize = 0)
        {
            // TODO: Replace with JWT token validation when implemented
            // var user = await ValidateUserFromToken();
            // userId = user.Id;
            
            // For now, validate user exists
            var userValidation = await ValidateUserExists(userId);
            if (!userValidation.IsValid)
            {
                return Unauthorized(new { message = userValidation.ErrorMessage });
            }

            pageSize = ValidatePageSize(pageSize);

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
        public async Task<ActionResult<TradeCompositeModel>> AddTrade(Guid userId)
        {
            // TODO: Replace with JWT token validation when implemented
            var userValidation = await ValidateUserExists(userId);
            if (!userValidation.IsValid)
            {
                return Unauthorized(new { message = userValidation.ErrorMessage });
            }

            var positionComposite = await _journalAccess.Journal.AddTradeCompositeAsync(userId);
            TradeCompositeModel resAsModel = _mapper.Map<TradeCompositeModel>(positionComposite);

            _cacheService.InvalidateAndReload(userId);

            return Ok(resAsModel);
        }

        [HttpGet("{tradeId}")]
        public async Task<ActionResult<TradeCompositeModel>> GetTradeById(int tradeId, Guid userId)
        {
            try
            {
                // TODO: Replace with JWT token validation when implemented
                var userValidation = await ValidateUserExists(userId);
                if (!userValidation.IsValid)
                {
                    return Unauthorized(new { message = userValidation.ErrorMessage });
                }

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

        private async Task<(bool IsValid, string? ErrorMessage)> ValidateUserExists(Guid userId)
        {
            try
            {
                var request = new GetUserRequest { UserId = userId.ToString() };
                var response = await _userServiceClient.GetUserByIdAsync(request);
                
                if (response.Success)
                {
                    return (true, null);
                }
                
                return (false, response.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating user: {UserId}", userId);
                return (false, "Error validating user");
            }
        }

        // TODO: Implement when JWT is added
        // private async Task<UserDto> ValidateUserFromToken()
        // {
        //     var token = Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
        //     if (string.IsNullOrEmpty(token))
        //     {
        //         throw new UnauthorizedAccessException("No token provided");
        //     }
        //     
        //     // Validate JWT token and extract user info
        //     // This will be implemented when JWT is added
        //     throw new NotImplementedException("JWT validation not yet implemented");
        // }
        #endregion
    }
}
