﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DayJT.Journal.Data;
using DayJT.Web.API.Models;
using DayJT.Journal.DataContext.Services;
using System.ComponentModel;

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
        public async Task<ActionResult<(TradeElementModel? newEntry, TradeElementModel? summary)>> AddInterimEntry(string tradeId, bool isAdd)
        {
            (TradeElement? newEntry, TradeElement? summary) entryAndSummary;
            if (isAdd)
            {
                entryAndSummary = await _journalAccess.AddPositionAsync(tradeId);
            }
            else
            {
                entryAndSummary = await _journalAccess.ReducePositionAsync(tradeId);
            }

            (TradeElementModel?, TradeElementModel?) resAsModel =
                            (_mapper.Map<TradeElementModel>(entryAndSummary.newEntry), _mapper.Map<TradeElementModel>(entryAndSummary.summary));

            return ResultHandling(resAsModel, $"Could not add interim element on : {tradeId}");
        }

        [HttpDelete("{tradeInputId}")]
        public async Task<ActionResult> DeleteInterimTradeInput(string tradeId, string tradeInputId)
        {
            TradeElement? summary = await _journalAccess.RemoveInterimEntry(tradeId, tradeInputId);

            if (summary ==null)
            {
                return NotFound();
            }

            TradeElementModel resAsModel = _mapper.Map<TradeElementModel>(summary);

            return ResultHandling(resAsModel, $"Could not delete interim element on : {tradeId}");
        }

        #endregion

        #region Closure

        [HttpPost("close/{closingPrice}")]
        public async Task<ActionResult<bool>> CloseTrade(string tradeId, string closingPrice)
        {
            await _journalAccess.CloseAsync(tradeId, closingPrice);

            return NoContent();
        }

        #endregion

    }
}
