using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using HsR.Journal.Seeder;

namespace AssetsFlowWeb.API.Controllers
{
    [ApiController]
    [Route("hsr-api/v{version:apiVersion}/admin")]
    public class AdminOperationsController : ControllerBase
    {
        private readonly DatabaseSeeder _seeder;

        public AdminOperationsController(DatabaseSeeder seeder)
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