using Microsoft.AspNetCore.Mvc;
using TraJedi.Journal.Data;
using TraJedi.Journal.Data.Services;

namespace TraJedi.Web.API.Controllers.Journal
{
    [ApiController]
    [Route("api/journal/trades/{tradeId}")]
    public class TradeInputsController : JournalControllerBase
    {
        public TradeInputsController(TradesRepository journalAccess, ILogger<JournalControllerBase> logger) : base(journalAccess, logger) { }

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
        public async Task<ActionResult<TradeInputModel>> NewTradeEntry(string tradeId)
        {
            try
            {
                TradeInputModel? newTradeInput = await _journalAccess.AddTradeEntryAsync(tradeId);

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
        public async Task<ActionResult<TradeInputModel>> NewTradeExit(string tradeId)
        {
            try
            {
                TradeInputModel? newTradeInput = await _journalAccess.AddTradeEntryAsync(tradeId);

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
        public async Task<ActionResult<InputComponentModel?>> UpdateComponent(string componentId, string newContent)
        {
            try
            {
                InputComponentModel? updatedComponent = await _journalAccess.UpdateTradeInputComponent(componentId, newContent);

                if (updatedComponent == null)
                {
                    _logger.LogWarning($"Could not update component: {componentId}");
                    return NotFound();
                }

                return Ok(updatedComponent);
            }
            catch (Exception ex)
            {
                return ExceptionHandling(ex, $"updating componentId: {componentId}");
            }
        }

        #endregion
    }
}
