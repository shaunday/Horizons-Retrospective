using Asp.Versioning;
using HsR.Journal.Seeder;
using HsR.UserService.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AssetsFlowWeb.API.Controllers
{
    [Route("hsr-api/v{version:apiVersion}/admin")]
    [ApiVersion("1.0")]
    [ApiController]
    //[Authorize(Roles = RoleNames.Admin)]
    public class AdminOperationsController : ControllerBase
    {
        private readonly DatabaseSeeder _seeder;
        private readonly ILogger<AdminOperationsController> _logger;

        public AdminOperationsController(DatabaseSeeder seeder, ILogger<AdminOperationsController> logger)
        {
            _seeder = seeder;
            _logger = logger;
        }

        public async Task<IActionResult> ReseedDemoUser()
        {
            _logger.LogInformation("ReseedDemoUser called");
            try
            {
                await _seeder.DeleteAllDemoUserTradesAsync();
                await _seeder.SeedDemoUserTradesAsync();
                _logger.LogInformation("ReseedDemoUser succeeded");
                return Ok(new { message = "Demo user and demo trades reseeded." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ReseedDemoUser failed");
                return StatusCode(500, "Internal server error");
            }
        }
    }
} 