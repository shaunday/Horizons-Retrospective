using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Xml.Linq;
using TraJedi.Journal.Data;
using TraJedi.Journal.Data.Services;

namespace TraJedi.Web.API.Controllers.Journal
{
    [Route("api/v{version:apiVersion}/journal/trades")]
    [ApiVersion("1.0")]
    [ApiController]
    public class TradesController : JournalControllerBase
    {
        const int maxTradesPageSize = 20;

        public TradesController(ITradesRepository journalAccess, ILogger<JournalControllerBase> logger) : base(journalAccess, logger) { }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradeConstruct>>> GetAllTrades(int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxTradesPageSize)
            {
                pageSize = maxTradesPageSize;
            }

            var (tradesEntities, paginationMetadata) = await _journalAccess.GetAllTradesAsync(pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(tradesEntities);
        }

        [HttpGet("trade1Liners")]
        public async Task<ActionResult<IEnumerable<TradeConstruct>>> GetAllTradesAs1Liner()
        {
            return Ok(await _journalAccess.GetAllTradesOneLinerSummariesAsync());
        }

        [HttpPost]
        public async Task<ActionResult<TradeInputModel>> AddTrade()
        {
            return Ok(await _journalAccess.AddTradeAsync());
        }

    }
}
