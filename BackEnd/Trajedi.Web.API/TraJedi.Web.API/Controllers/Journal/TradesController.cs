using Microsoft.AspNetCore.Mvc;
using TraJedi.Journal.Data;

namespace TraJedi.Web.API.Controllers.Journal
{
    [ApiController]
    [Route("api/journal/trades")]
    public class TradesController : JournalControllerBase
    {
        public TradesController(TradingJournalAccess journalAccess, ILogger<JournalControllerBase> logger) : base(journalAccess, logger) { }

        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllTrades()
        {
            return Ok(_journalAccess.Trades);
        }

        [HttpPost]
        public ActionResult<TradeInputModel> AddTrade()
        {
            return Ok(_journalAccess.AddTrade());
        }

    }
}
