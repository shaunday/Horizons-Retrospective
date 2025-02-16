using Asp.Versioning;
using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Web.Services.Models.Journal;
using Microsoft.AspNetCore.Mvc;

namespace HsR.Web.API.Controllers.Journal
{
    [Route("api/v{version:apiVersion}/journal/trades/{tradeId}")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CompositeController(IJournalRepositoryWrapper journalAccess, 
            ILogger<JournalControllerBase> logger, IMapper mapper) : JournalControllerBase(journalAccess, logger, mapper)
    {
        #region Interim positions

        [HttpPost]
        public async Task<ActionResult<(TradeElementModel newEntry, TradeElementModel? summary)>> AddReduceInterimPosition(string tradeId, bool isAdd)
        {
            (InterimTradeElement newEntry, TradeSummary? summary) entryAndSummary= await _journalAccess.AddInterimPositionAsync(tradeId, isAdd);

            (TradeElementModel, TradeElementModel) resAsModel =
                            (_mapper.Map<TradeElementModel>(entryAndSummary.newEntry), _mapper.Map<TradeElementModel>(entryAndSummary.summary));

            return ResultHandling(resAsModel, $"Could not add interim element on : {tradeId}", [NEW_ELEMENT_DATA, NEW_SUMMARY]);
        }

        [HttpPost("evaluate")]
        public async Task<ActionResult<TradeElementModel>> AddEvaluationPosition(string tradeId)
        {
            InterimTradeElement newEval = await _journalAccess.AddInterimEvalutationAsync(tradeId);

            TradeElementModel resAsModel = _mapper.Map<TradeElementModel>(newEval);

            return ResultHandling(resAsModel, $"Could not add new evaluation element on : {tradeId}");
        }

        [HttpDelete("{tradeInputId}")]
        public async Task<ActionResult<TradeElementModel>> DeleteInterimTradeInput(string tradeId, string tradeInputId)
        {
            var summary = await _journalAccess.RemoveInterimPositionAsync(tradeId, tradeInputId);
            if (summary == null)
            {
                return NotFound();
            }

            TradeElementModel resAsModel = _mapper.Map<TradeElementModel>(summary);

            return ResultHandling(resAsModel, $"Could not delete interim element on : {tradeId}");
        }

        #endregion

        #region Closure

        [HttpPost("close")]
        public async Task<ActionResult<TradeElementModel>> CloseTrade(string tradeId, [FromQuery] string closingPrice)
        {
            var summary = await _journalAccess.CloseTradeAsync(tradeId, closingPrice); 
            if (summary == null)
            {
                return NotFound();
            }

            TradeElementModel resAsModel = _mapper.Map<TradeElementModel>(summary);
            return ResultHandling(resAsModel, $"Could not close trade on : {tradeId}", [NEW_SUMMARY]);
        }

        #endregion
    }
}
