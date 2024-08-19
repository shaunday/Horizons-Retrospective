using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DayJT.Journal.Data;
using DayJT.Web.API.Models;
using DayJT.Journal.DataContext.Services;

namespace DayJT.Web.API.Controllers.Journal
{
    [Route("api/v{version:apiVersion}/journal/trades/{tradeId}/elements/{elementId}")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TradeElementController : JournalControllerBase
    {
        #region Ctor

        public TradeElementController(JournalRepository journalAccess, ILogger<JournalControllerBase> logger, IMapper mapper) :
                                                                                                        base(journalAccess, logger, mapper)
        { }
        #endregion

        [HttpGet]
        public async Task<ActionResult<(TradeElementModel? element, TradeElementModel? summary)>> GetTradeElement(string tradeId, string elementId)
        {
            (TradeElement? newEntry, TradeElement? summary) entryAndSummary = await _journalAccess.GetTradeElement(tradeId, elementId);

            (TradeElementModel?, TradeElementModel?) resAsModel =
                            (_mapper.Map<TradeElementModel>(entryAndSummary.newEntry), _mapper.Map<TradeElementModel>(entryAndSummary.summary));

            return Ok(resAsModel);
        }
    }
}
