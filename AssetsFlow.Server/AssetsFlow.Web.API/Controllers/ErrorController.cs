using Microsoft.AspNetCore.Mvc;

namespace AssetsFlowWeb.API.Controllers
{
    [ApiController]
    public class ErrorController : ControllerBase
    {
        [Route("error")]
        public IActionResult HandleError() => Problem("An unexpected error occurred.");
    }

}
