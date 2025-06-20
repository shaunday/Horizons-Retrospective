﻿using Asp.Versioning;
using AssetsFlowWeb.Services.Models.Journal;
using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Services;
using HsR.Web.API.Services;
using HsR.Web.Services.Models.Journal;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<ActionResult<(TradeElementModel newEntry, UpdatedStatesModel summary)>> 
                                                        AddReduceInterimPosition(string tradeId, [FromQuery] bool isAdd)
        {
            (InterimTradeElement newEntry, UpdatedStatesCollation? updatedStates) entryAndStates = 
                                                                    await _journalAccess.TradeElement.AddInterimPositionAsync(tradeId, isAdd);

            (TradeElementModel, UpdatedStatesModel) resAsModel =
                             (_mapper.Map<TradeElementModel>(entryAndStates.newEntry), _mapper.Map<UpdatedStatesModel>(entryAndStates.updatedStates));

            // Invalidate cache when a position is modified and start reload
            _cacheService.InvalidateAndReload();

            return ResultHandling(resAsModel, $"Could not add interim element on : {tradeId}", [NEW_ELEMENT_DATA, NEW_STATES_WRAPPER]);
        }

        [HttpPost("evaluate")]
        public async Task<ActionResult<TradeElementModel>> AddEvaluationPosition(string tradeId)
        {
            (InterimTradeElement newEval, UpdatedStatesCollation? updatedStates) newEvalAndStates = await _journalAccess.TradeElement.AddInterimEvalutationAsync(tradeId);

            (TradeElementModel, UpdatedStatesModel) resAsModel =
                             (_mapper.Map<TradeElementModel>(newEvalAndStates.newEval), _mapper.Map<UpdatedStatesModel>(newEvalAndStates.updatedStates));

            // Invalidate cache when an evaluation is added and start reload
            _cacheService.InvalidateAndReload();

            return ResultHandling(resAsModel, $"Could not add new evaluation element on : {tradeId}", [NEW_ELEMENT_DATA, NEW_STATES_WRAPPER]);
        }

        #endregion

        #region Closure

        [HttpPost("close")]
        public async Task<ActionResult<UpdatedStatesModel>> CloseTrade(string tradeId, [FromQuery] string closingPrice)
        {
            var updatedStates = await _journalAccess.TradeComposite.CloseTradeAsync(tradeId, closingPrice); 
            if (updatedStates == null)
            {
                return NotFound();
            }

            UpdatedStatesModel resAsModel = _mapper.Map<UpdatedStatesModel>(updatedStates);

            // Invalidate cache when a trade is closed and start reload
            _cacheService.InvalidateAndReload();

            return ResultHandling(resAsModel, $"Could not close trade on : {tradeId}", [NEW_STATES_WRAPPER]);
        }

        #endregion
    }
}
