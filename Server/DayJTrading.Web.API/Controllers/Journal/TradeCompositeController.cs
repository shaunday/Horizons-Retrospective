using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using Asp.Versioning;
using DayJT.Journal.DataEntities.Entities;
using DayJTrading.Web.Services.Models.Journal;
using DayJT.Journal.Repository.Services;

namespace DayJT.Web.API.Controllers.Journal
{
    [Route("api/v{version:apiVersion}/journal/trades/{tradeId}")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TradeCompositeController : JournalControllerBase
    {
        #region Ctor

        public TradeCompositeController(JournalRepository journalAccess, ILogger<JournalControllerBase> logger, IMapper mapper) :
                                                                                                        base(journalAccess, logger, mapper)
        { }
        #endregion

        #region Add / Delete

        [HttpPost]
        public async Task<ActionResult<(TradeElementModel newEntry, TradeElementModel summary)>> AddReduceInterimPosition(string tradeId, bool isAdd)
        {
            (TradeElement newEntry, TradeElement summary) entryAndSummary;
            entryAndSummary = await _journalAccess.AddInterimPositionAsync(tradeId, isAdd);

            (TradeElementModel, TradeElementModel) resAsModel =
                            (_mapper.Map<TradeElementModel>(entryAndSummary.newEntry), _mapper.Map<TradeElementModel>(entryAndSummary.summary));

            return ResultHandling(resAsModel, $"Could not add interim element on : {tradeId}");
        }

        [HttpDelete("{tradeInputId}")]
        public async Task<ActionResult> DeleteInterimTradeInput(string tradeId, string tradeInputId)
        {
            TradeElement summary = await _journalAccess.RemoveInterimPositionAsync(tradeId, tradeInputId);

            if (summary ==null)
            {
                return NotFound();
            }

            TradeElementModel resAsModel = _mapper.Map<TradeElementModel>(summary);

            return ResultHandling(resAsModel, $"Could not delete interim element on : {tradeId}");
        }

        #endregion

        #region Closure

        [HttpPost("close")]
        public async Task<ActionResult<(TradeElementModel? filler, TradeElementModel summary)>> CloseTrade(string tradeId, string closingPrice)
        {
            TradeElement summary = await _journalAccess.CloseTradeAsync(tradeId, closingPrice);

            if (summary == null)
            {
                return NotFound();
            }

            TradeElementModel resAsModel = _mapper.Map<TradeElementModel>(summary);
            TradeElement? filler = null;
            return ResultHandling((filler, resAsModel), $"Could not delete interim element on : {tradeId}");
        }

        #endregion

    }
}
