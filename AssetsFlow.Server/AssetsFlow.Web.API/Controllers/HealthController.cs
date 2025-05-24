using HsR.Journal.DataContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetsFlowWeb.API.Controllers
{
    [ApiController]
    [Route("hsr-api/v{version:apiVersion}/health")]
    public class HealthController(TradingJournalDataContext context, ILogger<HealthController> logger) : ControllerBase
    {
        private readonly TradingJournalDataContext _context = context;
        private readonly ILogger<HealthController> _logger = logger;

        [HttpGet("ping")]
        public async Task<IActionResult> Ping()
        {
            try
            {
                await _context.Database.ExecuteSqlRawAsync("SELECT 1");
                return Ok("pong");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Database unreachable in health check.");
                return StatusCode(503, "Database unreachable");
            }
        }
    }
}
