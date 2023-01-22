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
        #region Const

        public TradeInputsController(TradesRepository journalAccess, ILogger<JournalControllerBase> logger) : base(journalAccess, logger) { }

        #endregion

        #region Add / Remove

        [HttpPost]
        public async Task<ActionResult<(TradeInputModel newEntry, TradeInputModel summary)>> AddInterimEntry(string tradeId, bool isAdd)
        {
            (TradeInputModel newEntry, TradeInputModel summary) entryAndSummary;
            if (isAdd)
            {
                entryAndSummary = await _journalAccess.NewEntryAddPositionAsync(tradeId);
            }
            else
            {
                entryAndSummary = await _journalAccess.NewEntryReducePositionAsync(tradeId);
            }

            return Ok(entryAndSummary);
        }

        [HttpDelete("inputs/{tradeInputId}")]
        public async Task<ActionResult> DeleteInterimTradeInput(string tradeInputId)
        {
            (bool result, TradeInputModel? summary) = await _journalAccess.RemoveInterimEntry(tradeInputId);

            if (!result)
            {
                return NotFound();
            }

            return Ok(summary);
        }

        #endregion

        #region Update component

        [HttpPut("components/{componentId}")]
        public async Task<ActionResult<InputComponentModel?>> UpdateComponent(string componentId, string newContent, string changeNote)
        {
            InputComponentModel? updatedComponent = await _journalAccess.UpdateTradeInputComponent(componentId, newContent, changeNote);

            return ResultHandling(updatedComponent, $"Could not update component: {componentId}");
        }

        #endregion

        #region Closure

        [HttpPost("close/{closingPrice}")]
        public async Task<ActionResult<bool>> CloseTrade(string tradeId, string closingPrice)
        {
            await _journalAccess.Close(tradeId, closingPrice);

            return NoContent();
        }

        #endregion

    }
}
