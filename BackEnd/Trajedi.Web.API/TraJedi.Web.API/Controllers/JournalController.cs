using Microsoft.AspNetCore.Mvc;
using TraJedi.Journal.Data;
using TraJedi.Journal.Data.Wrappers;

namespace TraJedi.Web.API.Controllers
{
    [ApiController]
    [Route("api/journal/trades")]
    public class JournalController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable<string>> GetAllTrades()
        {
            return Ok(JournalWrapper.Current.Trades);
        }

        [HttpPost]
        public ActionResult<TradeInputModel> AddTrade()
        {
            return Ok(JournalWrapper.Current.AddTrade());
        }

    }
}
