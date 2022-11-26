using Microsoft.AspNetCore.Mvc;
using TraJedi.Journal.Data;
using TraJedi.Journal.Data.Services;

namespace TraJedi.Web.API.Controllers.Journal
{
    [Route("api/v{version:apiVersion}/journal/trades/{tradeId}")]
    [ApiVersion("1.0")]
    [ApiController]
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

        [HttpPost("newAddPosition")]
        public async Task<ActionResult<TradeInputModel>> NewAddPosition(string tradeId)
        {
            TradeInputModel? newTradeInput = await _journalAccess.NewEntryAddPositionAsync(tradeId);

            return ResultHandling(newTradeInput, $"Could not add entry with tradeId: {tradeId}");
        }

        [HttpPost("newReducePosition")]
        public async Task<ActionResult<TradeInputModel>> NewReducePosition(string tradeId)
        {
            TradeInputModel? newTradeInput = await _journalAccess.NewEntryReducePositionAsync(tradeId);

            return ResultHandling(newTradeInput, $"Could not add exit with tradeId: {tradeId}");
        }
        #endregion

        #region Remove

        [HttpDelete("{tradeInputId}")]
        public async Task<ActionResult> DeleteInterimTradeInput(string tradeInputId)
        {
            bool res = await _journalAccess.RemoveEntry(tradeInputId);

            if (!res)
            {
                return NotFound();
            }

            return NoContent();
        }

        #endregion

        #region Change

        [HttpPut("inputs/{tradeInputId}/components/{componentId}")]
        public async Task<ActionResult<InputComponentModel?>> UpdateComponent(string componentId, string newContent)
        {
            InputComponentModel? updatedComponent = await _journalAccess.UpdateTradeInputComponent(componentId, newContent);

            return ResultHandling(updatedComponent, $"Could not update component: {componentId}");
        }

        #endregion
    }
}
