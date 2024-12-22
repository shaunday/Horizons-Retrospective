using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Xml.Linq;
using JTA.Journal.Entities;
using JTA.Journal.DataContext.Services;
using Asp.Versioning;
using JTA.Web.Services.Models.Journal;

namespace JTA.Web.API.Controllers.Journal
{
    [Route("api/v{version:apiVersion}/journal/trades")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TradesJournalController : JournalControllerBase
    {
        const int maxTradesPageSize = 20;

        #region Ctor

        public TradesJournalController(IJournalRepository journalAccess, ILogger<JournalControllerBase> logger, IMapper mapper) :  
                                                                                                    base(journalAccess, logger, mapper) { }
        #endregion

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeCompositeModel>>> GetAllTrades(int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxTradesPageSize)
            {
                pageSize = maxTradesPageSize;
            }

            var (tradesEntities, paginationMetadata) = await _journalAccess.GetAllTradeCompositesAsync(pageNumber, pageSize);

            Response.Headers["X-Pagination"] = JsonSerializer.Serialize(paginationMetadata);

            IEnumerable<TradeCompositeModel> resAsModels = _mapper.Map<IEnumerable<TradeCompositeModel>>(tradesEntities);

            return Ok(resAsModels);
        }


        [HttpPost]
        public async Task<ActionResult<TradeCompositeModel>> AddTrade()
        {
            var positionComposite = await _journalAccess.AddTradeCompositeAsync();

            TradeCompositeModel resAsModel = _mapper.Map<TradeCompositeModel>(positionComposite);

            return Ok(resAsModel);
        }

    }
}
