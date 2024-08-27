using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Text.Json;
using System.Xml.Linq;
using DayJT.Journal.Data;
using DayJT.Web.API.Models;
using DayJT.Journal.DataContext.Services;

namespace DayJT.Web.API.Controllers.Journal
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

        [HttpPost]
        public async Task<ActionResult<TradeCompositeModel>> AddTrade()
        {
            var positionComposite = await _journalAccess.AddTradeCompositeAsync();

            TradeCompositeModel resAsModel = _mapper.Map<TradeCompositeModel>(positionComposite);

            return Ok(resAsModel);
        }

    }
}
