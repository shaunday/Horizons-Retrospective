using Asp.Versioning;
using HsR.Journal.DataContext;
using HsR.Web.API.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssetsFlowWeb.API.Controllers
{
    [Route("hsr-api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    public class HealthController(TradingJournalDataContext context, ILogger<HealthController> logger) : HsRControllerBase
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
