using Asp.Versioning;
using HsR.Journal.Seeder;
using HsR.UserService.Contracts;
using HsR.Web.API.Controllers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AssetsFlowWeb.API.Controllers
{
    [Route("hsr-api/v{version:apiVersion}/admin")]
    [ApiVersion("1.0")]
    [ApiController]
    //[Authorize(Roles = RoleNames.Admin)]
    public class AdminOperationsController : HsRControllerBase
    {
        private readonly DatabaseSeeder _seeder;

        public AdminOperationsController(DatabaseSeeder seeder, ILogger<HealthController> logger) : base(logger)
        {
            _seeder = seeder;
        }

        [HttpPost("reseed-demo-user")]
        public async Task<IActionResult> ReseedDemoUser()
        {
            await _seeder.DeleteAllDemoUserTradesAsync();
            await _seeder.SeedDemoUserTradesAsync();
            return Ok(new { message = "Demo user and demo trades reseeded." });
        }
    }
} 