using Asp.Versioning;
using HsR.Journal.Seeder;
using HsR.UserService.Contracts;
using HsR.Web.API.Controllers;
using HsR.Web.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AssetsFlowWeb.API.Controllers
{
    [Route("hsr-api/v{version:apiVersion}/admin")]
    [ApiVersion("1.0")]
    [ApiController]
    //[Authorize(Roles = RoleNames.Admin)]
    public class AdminOperationsController(DatabaseSeeder seeder, ITradesCacheService cacheService, ILogger<AdminOperationsController> logger) 
                                                                                                            : HsRControllerBase(logger)
    {
        private readonly DatabaseSeeder _seeder = seeder;
        private protected readonly ITradesCacheService _cacheService = cacheService;

        [HttpPost("reseed-demo-user")]
        public async Task<IActionResult> ReseedDemoUser()
        {
            await _seeder.DeleteAllDemoUserTradesAsync();
            await _seeder.SeedDemoUserTradesAsync();
            _cacheService.InvalidateAndReload(new Guid(DemoUserData.Id));
            return Ok(new { message = "Demo user and demo trades reseeded." });
        }
    }
} 