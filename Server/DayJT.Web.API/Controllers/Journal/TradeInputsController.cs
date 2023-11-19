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
    public class TradeInputsController : JournalControllerBase
    {
        #region Ctor

        public TradeInputsController(JournalRepository journalAccess, ILogger<JournalControllerBase> logger, IMapper mapper) :
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

            (TradeComponentModel?, TradeComponentModel?) resAsModel =
                            (_mapper.Map<TradeComponentModel>(entryAndSummary.newEntry), _mapper.Map<TradeComponentModel>(entryAndSummary.summary));

            return Ok(resAsModel);
        }

        [HttpDelete("{tradeInputId}")]
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
