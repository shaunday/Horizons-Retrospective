using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Xml.Linq;
using DayJT.Journal.Data;
using DayJT.Journal.Data.Services;
using DayJT.Web.API.Models;

namespace DayJT.Web.API.Controllers.Journal
{
    [Route("api/v{version:apiVersion}/journal/trades")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TradesPositionCompositeController : JournalControllerBase
    {
        const int maxTradesPageSize = 20;

        #region Ctor

        public TradesPositionCompositeController(IJournalRepository journalAccess, ILogger<JournalControllerBase> logger, IMapper mapper) :  
                                                                                                    base(journalAccess, logger, mapper) { }
        #endregion

        #region Getters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradePositionCompositeModel>>> GetAllTrades(int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxTradesPageSize)
            {
                pageSize = maxTradesPageSize;
            }

            var (tradesEntities, paginationMetadata) = await _journalAccess.GetAllTradeCompositesAsync(pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            IEnumerable<TradePositionCompositeModel> resAsModels = _mapper.Map<IEnumerable<TradePositionCompositeModel>>(tradesEntities);

            return Ok(resAsModels);
        }

        [HttpGet("tradeComponents")]
        public async Task<ActionResult<IEnumerable<TradeComponentModel>>> GetAllPositionComponents()
        {
            var tradeComponents = await _journalAccess.GetAllTradeCompositesAs1LinerOverviewAsync();

            IEnumerable<TradeComponentModel> resAsModels = _mapper.Map<IEnumerable<TradeComponentModel>>(tradeComponents);

            return Ok(resAsModels);
        } 
        #endregion

        [HttpPost]
        public async Task<ActionResult<TradePositionCompositeModel>> AddTrade()
        {
            var positionComposite = await _journalAccess.AddTradeCompositeAsync();

            TradePositionCompositeModel resAsModel = _mapper.Map<TradePositionCompositeModel>(positionComposite);

            return Ok(resAsModel);
        }

    }
}
