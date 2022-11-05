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
        public ActionResult<IEnumerable<string>> GetTradesIds()
        {
            return Ok(JournalWrapper.Current.GetTradeIds());
        }

        [HttpPost]
        public ActionResult<TradeInputModel> AddTrade()
        {
            return Ok(JournalWrapper.Current.AddTrade());
        }

    }
}
