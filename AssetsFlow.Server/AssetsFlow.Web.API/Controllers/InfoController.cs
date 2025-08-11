using Asp.Versioning;
using HsR.Common;
using HsR.Web.API.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace AssetsFlowWeb.API.Controllers
{
    [Route("hsr-api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [ResponseCache(Duration = 1800, Location = ResponseCacheLocation.Client)]
    public class InfoController(IWebHostEnvironment env, ILogger<HealthController> logger) : HsRControllerBase(logger)
    {
        private readonly IWebHostEnvironment _env = env;

        [HttpGet]
        public IActionResult GetInfo()
        {
            string? appVersion = Environment.GetEnvironmentVariable("APP_VERSION");
            string? buildTimeStamp = Environment.GetEnvironmentVariable("BUILD_TIMESTAMP");
            string? commitHash = Environment.GetEnvironmentVariable("COMMIT_SHA");

            _logger.LogError("Build time stamp = " + buildTimeStamp);

            if (!string.IsNullOrWhiteSpace(commitHash) && commitHash.Length > 7)
            {
                commitHash = commitHash[..7];
            }

            if (!string.IsNullOrWhiteSpace(buildTimeStamp))
                buildTimeStamp = buildTimeStamp.Replace("_", "T");
            else
                buildTimeStamp = DateTime.Now.ToString("o");


            if (_env.IsDevelopment())
            {
                // fallback to git commands in dev
                try
                {
                    commitHash = GitWrappers.RunGitCommand("rev-parse --short HEAD");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting git commit hash: {ex}");
                    commitHash = "unknown";
                }

                if (string.IsNullOrWhiteSpace(appVersion))
                {
                    try
                    {
                        appVersion = GitWrappers.RunGitCommand("describe --tags --abbrev=0");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error getting git tag: {ex}");
                        appVersion = "dev-unknown";
                    }
                }
            }

            return Ok(new
            {
                beVersion = appVersion ?? "unknown",
                buildTimeStamp = buildTimeStamp ?? "unknown",
                gitCommit = commitHash ?? "unknown"
            });
        }
    }
}
