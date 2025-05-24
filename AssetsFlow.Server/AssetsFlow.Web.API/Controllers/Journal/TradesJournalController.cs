using Asp.Versioning;
using AutoMapper;
using HsR.Common;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Web.Services.Models.Journal;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace HsR.Web.API.Controllers.Journal
{
    [Route("hsr-api/v{version:apiVersion}/journal/trades")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TradesJournalController(IJournalRepositoryWrapper journalAccess, 
            ILogger<JournalControllerBase> logger, IMapper mapper) : JournalControllerBase(journalAccess, logger, mapper)
    {
        const int maxTradesPageSize = 20;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeCompositeModel>>> GetAllTrades(int pageNumber = 1, int pageSize = 10)
        {
            pageSize = ValidatePageSize(pageSize);

            var (tradeEntities, paginationMetadata) = await _journalAccess.Journal.GetAllTradeCompositesAsync(pageNumber, pageSize);

            SetPaginationHeader(paginationMetadata);

            IEnumerable<TradeCompositeModel> resAsModels = _mapper.Map<IEnumerable<TradeCompositeModel>>(tradeEntities);

            return Ok(resAsModels);
        }

        [HttpGet("byFilter")]
        public async Task<ActionResult<IEnumerable<TradeCompositeModel>>> 
                                                    GetFilteredTrades(TradesFilterModel filter, int pageNumber = 1, int pageSize = 10)
        {
            pageSize = ValidatePageSize(pageSize);

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

        #endregion

    }
}
