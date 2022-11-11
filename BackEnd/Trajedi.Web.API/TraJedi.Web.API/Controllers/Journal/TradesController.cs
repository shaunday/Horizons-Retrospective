using Microsoft.AspNetCore.Mvc;
using TraJedi.Journal.Data;
using TraJedi.Journal.Data.Services;

namespace TraJedi.Web.API.Controllers.Journal
{
    [ApiController]
    [Route("api/journal/trades")]
    public class TradesController : JournalControllerBase
    {
        public TradesController(ITradesRepository journalAccess, ILogger<JournalControllerBase> logger) : base(journalAccess, logger) { }

        [HttpGet]
        public ActionResult<IEnumerable<TradeConstruct>> GetAllTrades()
        {
            return Ok(_journalAccess.GetAllTradesAsync());
        }

        [HttpPost]
        public ActionResult<TradeInputModel> AddTrade()
        {
            return Ok(_journalAccess.AddTradeAsync());
        }

    }
}
