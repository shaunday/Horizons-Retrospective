using Microsoft.AspNetCore.Mvc;
using TraJedi.Journal.Data.Wrappers;
using TraJedi.Journal.Data;
using Microsoft.Extensions.Logging.Abstractions;

namespace TraJedi.Web.API.Controllers
{
    [ApiController]
    [Route("api/journal/trades/{tradeId}")]
    public class TradeInputsController : Controller
    {
        #region Get

        [HttpGet("origin")]
        public ActionResult<TradeInputModel> GetTradeOrigin(string tradeId)
        {
            TradeInputModel? tradeInputToReturn = JournalWrapper.Current.GetTrade(tradeId)?.GetTradeOrigin();
            if (tradeInputToReturn == null)
            {
                return NotFound();
            }
            return Ok(tradeInputToReturn);
        }

        [HttpGet("interims")]
        public ActionResult<IEnumerable<TradeInputModel>>? GetTradeInterims(string tradeId)
        {
            IEnumerable<TradeInputModel>? tradeInputsToReturn = JournalWrapper.Current.GetTrade(tradeId)?.GetTradeInterims();

            if (tradeInputsToReturn != null && !tradeInputsToReturn.Any())
            {
                return NotFound();
            }
            return Ok(tradeInputsToReturn);
        }

        [HttpGet("closure")]
        public ActionResult<TradeInputModel> GetTradeClosure(string tradeId)
        {
            TradeInputModel? tradeInputToReturn = JournalWrapper.Current.GetTrade(tradeId)?.GetTradeClosure();
            if (tradeInputToReturn == null)
            {
                return NotFound();
            }
            return Ok(tradeInputToReturn);
        }

        #endregion

        #region Add

        [HttpGet("newEntry")]
        public ActionResult<TradeInputModel> NewTradeEntry(string tradeId)
        {
            TradeInputModel newTradeInput = null;

            TradeWrapper? tradeWrapper = JournalWrapper.Current.GetTrade(tradeId);
            if (tradeWrapper != null)
            {
                newTradeInput = tradeWrapper.AddTradeEntry();
            }

            if (newTradeInput == null)
            {
                return NotFound();
            }
            return Ok(newTradeInput);
        }

        [HttpGet("newExit")]
        public ActionResult<TradeInputModel> NewTradeExit(string tradeId)
        {
            TradeInputModel newTradeInput = null;

            TradeWrapper? tradeWrapper = JournalWrapper.Current.GetTrade(tradeId);
            if (tradeWrapper != null)
            {
                newTradeInput = tradeWrapper.AddTradeExit();
            }

            if (newTradeInput == null)
            {
                return NotFound();
            }
            return Ok(newTradeInput);
        }
        #endregion

        #region Change

        [HttpPut("inputs/{tradeInputId}/components/{componentId}")]
        public ActionResult<InputComponentModel> UpdateComponent(string tradeId, string tradeInputId, string componentId, string newContent)
        {
            TradeWrapper? tradeWrapper = JournalWrapper.Current.GetTrade(tradeId);
            InputComponentModel? updatedComponent = null;

            if (tradeWrapper != null)
            {
                updatedComponent = tradeWrapper.UpdateTradeInputComponent(tradeInputId, componentId, newContent);
            }

            if (updatedComponent != null)
            {
                return Ok(updatedComponent);
            }

            return NotFound();
        }

        #endregion
    }
}
