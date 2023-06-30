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
        #region Const
        const int maxTradesPageSize = 20;

        public TradesController(IJournalRepository journalAccess, ILogger<JournalControllerBase> logger) : base(journalAccess, logger) { }

        #endregion

        #region Getters
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TradePositionComposite>>> GetAllTrades(int pageNumber = 1, int pageSize = 10)
        {
            if (pageSize > maxTradesPageSize)
            {
                pageSize = maxTradesPageSize;
            }

            var (tradesEntities, paginationMetadata) = await _journalAccess.GetAllTradeCompositesAsync(pageNumber, pageSize);

            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

            return Ok(tradesEntities);
        }

        [HttpGet("tradeOverviews")]
        public async Task<ActionResult<IEnumerable<TradePositionComposite>>> GetAllTradeOverviews()
        {
            return Ok(await _journalAccess.GetAllTradeInfoLinesAsync());
        } 
        #endregion

        [HttpPost]
        public async Task<ActionResult<TradePositionComposite>> AddTrade()
        {
            return Ok(await _journalAccess.AddTradeCompositeAsync());
        }

    }
}
