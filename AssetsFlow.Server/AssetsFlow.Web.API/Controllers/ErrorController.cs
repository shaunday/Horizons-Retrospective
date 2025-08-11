using HsR.Web.API.Controllers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AssetsFlowWeb.API.Controllers
{
    [ApiController]
    public class ErrorController : HsRControllerBase
    {
        public ErrorController(ILogger<ErrorController> logger) :base(logger) { }

        [Route("hsr-api/error")]
        public IActionResult HandleError()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerFeature>();
            if (exceptionFeature != null)
            {
                var exception = exceptionFeature.Error;
                _logger.LogError(exception, "Unhandled exception occurred.");
            }

            return Problem("An unexpected error occurred.");
        }
    }
}
