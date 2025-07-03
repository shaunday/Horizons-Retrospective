using Asp.Versioning;
using AssetsFlowWeb.Services.Models.Journal;
using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Services;
using HsR.Web.API.Services;
using HsR.Web.Services.Models.Journal;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;

namespace HsR.Web.API.Controllers.Journal
{
    [Route("hsr-api/v{version:apiVersion}/journal/trades/{tradeId}")]
    [ApiVersion("1.0")]
    [ApiController]
    public class CompositeController : JournalControllerBase
    {
        public CompositeController(
            IJournalRepositoryWrapper journalAccess,
            ILogger<JournalControllerBase> logger,
            IMapper mapper,
            ITradesCacheService cacheService) : base(journalAccess, logger, mapper, cacheService)
        {
        }

        #region Interim positions

        [HttpPost]
        public async Task<ActionResult<(TradeElementModel newEntry, UpdatedStatesModel summary)>> AddReduceInterimPosition(int tradeId, bool isAdd)
        {
            (InterimTradeElement newEntry, UpdatedStatesCollation? updatedStates) entryAndStates = 
                                                                    await _journalAccess.TradeElement.AddInterimPositionAsync(tradeId, isAdd);
            _cacheService.InvalidateAndReload(entryAndStates.newEntry.UserId);

            (TradeElementModel, UpdatedStatesModel) resAsModel =
                             (_mapper.Map<TradeElementModel>(entryAndStates.newEntry), _mapper.Map<UpdatedStatesModel>(entryAndStates.updatedStates));

            return ResultHandling(resAsModel, $"Could not add interim element on : {tradeId}", [NEW_ELEMENT_DATA, NEW_STATES_WRAPPER]);
        }

        [HttpPost("evaluate")]
        public async Task<ActionResult<TradeElementModel>> AddEvaluationPosition(int tradeId)
        {
            (InterimTradeElement newEval, UpdatedStatesCollation? updatedStates) newEvalAndStates = await _journalAccess.TradeElement.AddInterimEvalutationAsync(tradeId);
            _cacheService.InvalidateAndReload(newEvalAndStates.newEval.UserId);

            (TradeElementModel, UpdatedStatesModel) resAsModel =
                             (_mapper.Map<TradeElementModel>(newEvalAndStates.newEval), _mapper.Map<UpdatedStatesModel>(newEvalAndStates.updatedStates));

            return ResultHandling(resAsModel, $"Could not add new evaluation element on : {tradeId}", [NEW_ELEMENT_DATA, NEW_STATES_WRAPPER]);
        }

        #endregion

        #region Closure

        [HttpPost("close")]
        public async Task<ActionResult<UpdatedStatesModel>> CloseTrade(int tradeId, string closingPrice)
        {
            var updatedTrade = await _journalAccess.TradeComposite.CloseTradeAsync(tradeId, closingPrice);
            _cacheService.InvalidateAndReload(updatedTrade.UserId);

            UpdatedStatesCollation updatedStates = new() { TradeInfo = updatedTrade };
            UpdatedStatesModel resAsModel = _mapper.Map<UpdatedStatesModel>(updatedStates);

            return ResultHandling(resAsModel, $"Could not close trade on : {tradeId}", [NEW_STATES_WRAPPER]);
        }

        #endregion
    }
}
