using Asp.Versioning;
using AutoMapper;
using JTA.Common;
using JTA.Journal.DataContext;
using JTA.Journal.Entities;
using JTA.Web.Services.Models.Journal;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace JTA.Web.API.Controllers.Journal
{
    [Route("api/v{version:apiVersion}/journal/trades")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TradeComponentsController : JournalControllerBase
    {
        const int maxTradesPageSize = 20;

        #region Ctor

        public TradeComponentsController(IJournalRepository journalAccess, ILogger<JournalControllerBase> logger, IMapper mapper) :  
                                                                                                    base(journalAccess, logger, mapper) { }
        #endregion

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeCompositeModel>>> GetAllTrades(int pageNumber = 1, int pageSize = 10)
        {
            pageSize = ValidatePageSize(pageSize);

            var (tradesEntities, paginationMetadata) = await _journalAccess.GetAllTradeCompositesAsync(pageNumber, pageSize);

            SetPaginationHeader(paginationMetadata);

            // Map trade entities to models asynchronously
            var resAsModels = await MapToModelAsync(tradesEntities);

            return Ok(resAsModels);
        }

        [HttpGet("byFilter")]
        public async Task<ActionResult<IEnumerable<TradeCompositeModel>>> GetFilteredTrades(TradesFilterModel filter, int pageNumber = 1, int pageSize = 10)
        {
            pageSize = ValidatePageSize(pageSize);

            var (filteredTradesEntities, paginationMetadata) = await _journalAccess.GetFilteredTradesAsync(filter, pageNumber, pageSize);

            SetPaginationHeader(paginationMetadata);

            // Map filtered trade entities to models asynchronously
            var resAsModels = await MapToModelAsync(filteredTradesEntities);

            return Ok(resAsModels);
        }

        [HttpPost]
        public async Task<ActionResult<TradeCompositeModel>> AddTrade()
        {
            var positionComposite = await _journalAccess.AddTradeCompositeAsync();

            TradeCompositeModel resAsModel = _mapper.Map<TradeCompositeModel>(positionComposite);

            return Ok(resAsModel);
        }


        #region Helper methods

        private int ValidatePageSize(int pageSize)
        {
            return pageSize > maxTradesPageSize ? maxTradesPageSize : pageSize;
        }

        private void SetPaginationHeader(PaginationMetadata paginationMetadata)
        {
            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(paginationMetadata);
        }

        private async Task<List<TradeCompositeModel>> MapToModelAsync(IAsyncEnumerable<TradeComposite> tradesEntities)
        {
            var resAsModels = new List<TradeCompositeModel>();

            await foreach (var tradeEntity in tradesEntities)
            {
                resAsModels.Add(_mapper.Map<TradeCompositeModel>(tradeEntity));
            }

            return resAsModels;
        }
        

        #endregion

    }
}
