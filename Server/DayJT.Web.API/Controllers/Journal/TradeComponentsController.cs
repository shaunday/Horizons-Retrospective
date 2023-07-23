using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using DayJT.Journal.Data;
using DayJT.Journal.Data.Services;
using DayJT.Web.API.Models;

namespace DayJT.Web.API.Controllers.Journal
{
    [Route("api/v{version:apiVersion}/journal/trades/{tradeId}")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TradeComponentsController : JournalControllerBase
    {
        #region Ctor

        public TradeComponentsController(JournalRepository journalAccess, ILogger<JournalControllerBase> logger, IMapper mapper) :
                                                                                                        base(journalAccess, logger, mapper)
        { }
        #endregion

        #region Add / Remove

        [HttpPost]
        public async Task<ActionResult<(TradeComponentModel? newEntry, TradeComponentModel? summary)>> AddInterimEntry(string tradeId, bool isAdd)
        {
            (TradeComponent? newEntry, TradeComponent? summary) entryAndSummary;
            if (isAdd)
            {
                entryAndSummary = await _journalAccess.NewEntryAddPositionAsync(tradeId);
            }
            else
            {
                entryAndSummary = await _journalAccess.NewEntryReducePositionAsync(tradeId);
            }

            (TradeComponentModel? newEntry, TradeComponentModel? TradeComponentModel) resAsModel =
                            (_mapper.Map<TradeComponentModel>(entryAndSummary.newEntry), _mapper.Map<TradeComponentModel>(entryAndSummary.summary));

            return Ok(resAsModel);
        }

        [HttpDelete("inputs/{tradeInputId}")]
        public async Task<ActionResult> DeleteInterimTradeInput(string tradeId, string tradeInputId)
        {
            (bool result, TradeComponent? summary) = await _journalAccess.RemoveInterimEntry(tradeId, tradeInputId);

            if (!result)
            {
                return NotFound();
            }

            TradeComponentModel resAsModel = _mapper.Map<TradeComponentModel>(summary);

            return Ok(resAsModel);
        }

        #endregion

        #region Update component

        [HttpPut("inputs/{tradeInputId}/components/{componentId}")]
        public async Task<ActionResult> UpdateComponent(string tradeId, string tradeInputId, string componentId, string newContent, string changeNote)
        {
            Cell? updatedComponent = await _journalAccess.UpdateCellContent(componentId, newContent, changeNote);

            CellModel? resAsModel = null;

            if (updatedComponent != null)
            {
                resAsModel = _mapper.Map<CellModel>(updatedComponent);
            }

            return ResultHandling(resAsModel, $"Could not update component: {componentId}");
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
