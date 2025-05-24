using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace AssetsFlowWeb.API.Controllers
{
    [ApiController]
    [Route("api/info")]
    public class InfoController : ControllerBase
    {
        private readonly ApiVersioningOptions _apiVersioningOptions;

        public InfoController(IOptions<ApiVersioningOptions> apiVersioningOptions)
        {
            _apiVersioningOptions = apiVersioningOptions.Value;
        }

        [HttpGet]
        public IActionResult GetInfo()
        {
            var appVersion = Environment.GetEnvironmentVariable("APP_VERSION") ?? "unknown";

            var defaultApiVersion = _apiVersioningOptions.DefaultApiVersion?.ToString() ?? "none";

            return Ok(new
            {
                image_tag = appVersion,
                apiVersion = defaultApiVersion
            });
        }
    }
}
