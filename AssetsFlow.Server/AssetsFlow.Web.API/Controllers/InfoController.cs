using Asp.Versioning;
using HsR.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting; // Needed for IWebHostEnvironment

namespace AssetsFlowWeb.API.Controllers
{
    [Route("hsr-api/v{version:apiVersion}/info")]
    [ApiVersion("1.0")]
    [ApiController]
    public class InfoController(IWebHostEnvironment env) : ControllerBase
    {
        private readonly IWebHostEnvironment _env = env;

        [HttpGet]
        public IActionResult GetInfo()
        {
            string? appVersion = Environment.GetEnvironmentVariable("APP_VERSION");
            string? buildTimeStamp = Environment.GetEnvironmentVariable("BUILD_TIMESTAMP");
            string? commitHash = Environment.GetEnvironmentVariable("COMMIT_SHA");

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
                catch
                {
                    commitHash = "unknown";
                }

                if (string.IsNullOrWhiteSpace(appVersion))
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
