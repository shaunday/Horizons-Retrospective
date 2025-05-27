using Asp.Versioning;
using HsR.Common;
using Microsoft.AspNetCore.Mvc;

namespace AssetsFlowWeb.API.Controllers
{
    [Route("hsr-api/v{version:apiVersion}/info")]
    [ApiVersion("1.0")]
    [ApiController]
    public class InfoController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetInfo()
        {
            string? appVersion = Environment.GetEnvironmentVariable("APP_VERSION");
            string? commitHash = "unknown";

            // Always try to get commit hash
            try
            {
                commitHash = GitWrappers.RunGitCommand("rev-parse --short HEAD");
            }
            catch
            {
                commitHash = "unknown";
            }

            // If in development and APP_VERSION not set, fall back to last Git tag
            var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            if (string.IsNullOrWhiteSpace(appVersion) && env == "Development")
            {
                try
                {
                    appVersion = GitWrappers.RunGitCommand("describe --tags --abbrev=0");
                }
                catch
                {
                    appVersion = "dev-unknown";
                }
            }

            return Ok(new
            {
                beVersion = appVersion ?? "unknown",
                gitCommit = commitHash
            });
        }
    }
}
