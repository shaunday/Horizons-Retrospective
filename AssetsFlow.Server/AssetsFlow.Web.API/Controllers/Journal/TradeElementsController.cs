﻿using Asp.Versioning;
using AutoMapper;
using HsR.Journal.DataContext;
using HsR.Journal.Entities;
using HsR.Web.Services.Models.Journal;
using Microsoft.AspNetCore.Mvc;

namespace HsR.Web.API.Controllers.Journal
{
    [Route("api/v{version:apiVersion}/journal/trades/{tradeId}")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TradeElementsController(IJournalRepositoryWrapper journalAccess, 
            ILogger<JournalControllerBase> logger, IMapper mapper) : JournalControllerBase(journalAccess, logger, mapper)
    {

        #region Add / Delete

        [HttpPost]
        public async Task<ActionResult<(TradeElementModel newEntry, TradeElementModel? summary)>> AddReduceInterimPosition(string tradeId, bool isAdd)
        {
            (TradeElement newEntry, TradeElement? summary) entryAndSummary;
            entryAndSummary = await _journalAccess.AddInterimPositionAsync(tradeId, isAdd);

            (TradeElementModel, TradeElementModel) resAsModel =
                            (_mapper.Map<TradeElementModel>(entryAndSummary.newEntry), _mapper.Map<TradeElementModel>(entryAndSummary.summary));

            return ResultHandling(resAsModel, $"Could not add interim element on : {tradeId}");
        }

        [HttpDelete("{tradeInputId}")]
        public async Task<ActionResult<TradeElementModel>> DeleteInterimTradeInput(string tradeId, string tradeInputId)
        {
            TradeElement? summary = await _journalAccess.RemoveInterimPositionAsync(tradeId, tradeInputId);

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
        internal async Task<ActionResult<(TradeElementModel? filler, TradeElementModel summary)>> CloseTrade(string tradeId, string closingPrice)
        {
            TradeElement summary = await _journalAccess.CloseTradeAsync(tradeId, closingPrice);

            if (summary == null)
            {
                return NotFound();
            }

            TradeElementModel resAsModel = _mapper.Map<TradeElementModel>(summary);
            TradeElement? filler = null;
            return ResultHandling((filler, resAsModel), $"Could not close trade on : {tradeId}");
        }

        #endregion

        #region Activate

        [HttpPost("activate")]
        internal async Task<ActionResult<TradeElementModel>> ActivateTradeElment(string tradeId, string tradeEleId)
        {
            TradeElement tradeElement = await _journalAccess.ActivateTradeElement(tradeEleId);

            if (tradeElement == null)
            {
                return NotFound();
            }

            TradeElementModel resAsModel = _mapper.Map<TradeElementModel>(tradeElement);
            return ResultHandling((resAsModel), $"Could not activate element on : {tradeEleId}");
        }

        #endregion
    }
}
