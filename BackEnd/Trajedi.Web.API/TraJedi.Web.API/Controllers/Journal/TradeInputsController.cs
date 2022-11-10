using Microsoft.AspNetCore.Mvc;
using TraJedi.Journal.Data;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.AspNetCore.StaticFiles;
using System.ComponentModel;

namespace TraJedi.Web.API.Controllers.Journal
{
    [ApiController]
    [Route("api/journal/trades/{tradeId}")]
    public class TradeInputsController : JournalControllerBase
    {
        public TradeInputsController(TradingJournalAccess journalAccess, ILogger<JournalControllerBase> logger) : base(journalAccess, logger) { }

        //#region Get

        //[HttpGet("origin")]
        //public ActionResult<TradeInputModel> GetTradeOrigin(string tradeId)
        //{
        //    TradeInputModel? tradeInputToReturn = JournalWrapper.Current.GetTrade(tradeId)?.GetTradeOrigin();
        //    if (tradeInputToReturn == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(tradeInputToReturn);
        //}

        //[HttpGet("interims")]
        //public ActionResult<IEnumerable<TradeInputModel>>? GetTradeInterims(string tradeId)
        //{
        //    IEnumerable<TradeInputModel>? tradeInputsToReturn = JournalWrapper.Current.GetTrade(tradeId)?.GetTradeInterims();

        //    if (tradeInputsToReturn != null && !tradeInputsToReturn.Any())
        //    {
        //        return NotFound();
        //    }
        //    return Ok(tradeInputsToReturn);
        //}

        //[HttpGet("closure")]
        //public ActionResult<TradeInputModel> GetTradeClosure(string tradeId)
        //{
        //    TradeInputModel? tradeInputToReturn = JournalWrapper.Current.GetTrade(tradeId)?.GetTradeClosure();
        //    if (tradeInputToReturn == null)
        //    {
        //        return NotFound();
        //    }
        //    return Ok(tradeInputToReturn);
        //}

        //#endregion

        #region Add

        [HttpGet("newEntry")]
        public ActionResult<TradeInputModel> NewTradeEntry(string tradeId)
        {
            try
            {
                TradeInputModel newTradeInput = null;

                TradeWrapper? tradeWrapper = _journalAccess.GetTrade(tradeId);
                if (tradeWrapper != null)
                {
                    newTradeInput = tradeWrapper.AddTradeEntry();
                }

                if (newTradeInput == null)
                {
                    _logger.LogWarning($"Could not add entry with tradeId: {tradeId}");
                    return NotFound();
                }
                return Ok(newTradeInput);
            }
            catch (Exception ex)
            {
                return ExceptionHandling(ex, $"adding an entry with tradeId: {tradeId}");
            }
        }

        [HttpGet("newExit")]
        public ActionResult<TradeInputModel> NewTradeExit(string tradeId)
        {
            try
            {
                TradeInputModel newTradeInput = null;

                TradeWrapper? tradeWrapper = _journalAccess.GetTrade(tradeId);
                if (tradeWrapper != null)
                {
                    newTradeInput = tradeWrapper.AddTradeExit();
                }

                if (newTradeInput == null)
                {
                    _logger.LogWarning($"Could not add exit with tradeId: {tradeId}");
                    return NotFound();
                }
                return Ok(newTradeInput);
            }
            catch (Exception ex)
            {
                return ExceptionHandling(ex, $"adding an exit with tradeId: {tradeId}");
            }
        }
        #endregion

        #region Change

        [HttpPut("inputs/{tradeInputId}/components/{componentId}")]
        public ActionResult<InputComponentModel> UpdateComponent(string tradeId, string tradeInputId, string componentId, string newContent)
        {
            try
            {
                TradeWrapper? tradeWrapper = _journalAccess.GetTrade(tradeId);
                InputComponentModel? updatedComponent = null;

                if (tradeWrapper != null)
                {
                    updatedComponent = tradeWrapper.UpdateTradeInputComponent(tradeInputId, componentId, newContent);
                }

                if (updatedComponent != null)
                {
                    return NotFound();
                }

                _logger.LogWarning($"Could not update component with tradeId: {tradeId}, componentId: {componentId}");
                return Ok(updatedComponent);
            }
            catch (Exception ex)
            {
                return ExceptionHandling(ex, $"updating component with tradeId: {tradeId}, componentId: {componentId}");
            }
        }

        #endregion
    }
}
