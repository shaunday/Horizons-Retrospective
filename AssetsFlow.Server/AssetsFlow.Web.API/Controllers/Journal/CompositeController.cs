using Asp.Versioning;
using HsR.Web.Services.Models.Journal;
using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Journal.Entities.TradeJournal;
using HsR.Journal.Services;
using HsR.Web.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Diagnostics;
using Serilog;
using Microsoft.AspNetCore.Authorization;
using AssetsFlowWeb.Services.Models.Journal;

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
            try
            {
                InterimTradeElement newEntry = await _journalAccess.TradeElement.AddInterimPositionAsync(tradeId, isAdd);
                _cacheService.InvalidateAndReload(newEntry.UserId);
                UpdatedStatesCollation updatedStates = new UpdatedStatesCollation() { TradeInfo = newEntry.CompositeRef };
                (TradeElementModel, UpdatedStatesModel) resAsModel =
                                 (_mapper.Map<TradeElementModel>(newEntry), _mapper.Map<UpdatedStatesModel>(updatedStates));
                return ResultHandling(resAsModel, $"Could not add interim element on : {tradeId}", [NEW_ELEMENT_DATA, NEW_STATES_WRAPPER]);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error adding interim position for tradeId: {TradeId}", tradeId);
                var (status, msg) = ExceptionMappingService.MapToStatusCode(ex);
                return StatusCode(status, msg);
            }
        }

        [HttpPost("evaluate")]
        public async Task<ActionResult<TradeElementModel>> AddEvaluationPosition(int tradeId)
        {
            try
            {
                InterimTradeElement newEval = await _journalAccess.TradeElement.AddInterimEvalutationAsync(tradeId);
                _cacheService.InvalidateAndReload(newEval.UserId);
                UpdatedStatesCollation updatedStates = new UpdatedStatesCollation() { TradeInfo = newEval.CompositeRef };
                (TradeElementModel, UpdatedStatesModel) resAsModel =
                                 (_mapper.Map<TradeElementModel>(newEval), _mapper.Map<UpdatedStatesModel>(updatedStates));
                return ResultHandling(resAsModel, $"Could not add new evaluation element on : {tradeId}", [NEW_ELEMENT_DATA, NEW_STATES_WRAPPER]);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error adding evaluation position for tradeId: {TradeId}", tradeId);
                var (status, msg) = ExceptionMappingService.MapToStatusCode(ex);
                return StatusCode(status, msg);
            }
        }

        #endregion

        #region Closure

        [HttpPost("close")]
        public async Task<ActionResult<UpdatedStatesModel>> CloseTrade(int tradeId, string closingPrice)
        {
            try
            {
                (TradeComposite updatedTrade, InterimTradeElement? newReduceElement) updatedTradeData = await _journalAccess.TradeComposite.CloseTradeAsync(tradeId, closingPrice);
                _cacheService.InvalidateAndReload(updatedTradeData.updatedTrade?.UserId ?? Guid.Empty);
                UpdatedStatesCollation updatedStates = new() { TradeInfo = updatedTradeData.updatedTrade };
                (TradeElementModel, UpdatedStatesModel) resAsModel =
                                 (_mapper.Map<TradeElementModel>(updatedTradeData.newReduceElement), _mapper.Map<UpdatedStatesModel>(updatedStates));
                return ResultHandling(resAsModel, $"Could not close trade on : {tradeId}", [NEW_ELEMENT_DATA, NEW_STATES_WRAPPER]);
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Error closing trade for tradeId: {TradeId}", tradeId);
                var (status, msg) = ExceptionMappingService.MapToStatusCode(ex);
                return StatusCode(status, msg);
            }
        }

        #endregion
    }
}
