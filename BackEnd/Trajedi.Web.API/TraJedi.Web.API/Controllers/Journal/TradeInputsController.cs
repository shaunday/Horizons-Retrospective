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

        #region Get

        [HttpGet("inputs/summary")]
        public async Task<ActionResult<TradeInputModel>> GetTradeSummary(string tradeId)
        {
            TradeInputModel? tradeInput = await _journalAccess.GetTradeSummaryAsync(tradeId);

            return ResultHandling(tradeInput, $"Could not get trade summary for tradeId: {tradeId}");
        }

        #endregion

        #region Add

        [HttpPost]
        public async Task<ActionResult<TradeInputModel>> NewEntry(string tradeId, bool isAdd) 
        {
            TradeInputModel? newTradeInput;
            if (isAdd)
            {
                newTradeInput = await _journalAccess.NewEntryAddPositionAsync(tradeId);
            }
            else
            {
                newTradeInput = await _journalAccess.NewEntryReducePositionAsync(tradeId);
            }

            string addOrReduce = isAdd ? "add" : "reduce";

            return ResultHandling(newTradeInput, $"Could not {addOrReduce} position to tradeId: {tradeId}");
        }

        #endregion

        #region Remove

        [HttpDelete("inputs/{tradeInputId}")]
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

        #region Change component

        [HttpPut("inputs/{tradeInputId}/components/{componentId}")]
        public async Task<ActionResult<InputComponentModel?>> UpdateComponent(string componentId, string newContent)
        {
            InputComponentModel? updatedComponent = await _journalAccess.UpdateTradeInputComponent(componentId, newContent);

            return ResultHandling(updatedComponent, $"Could not update component: {componentId}");
        }

        #endregion
    }
}
