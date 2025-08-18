using Asp.Versioning;
using HsR.Common;
using HsR.Web.API.Controllers;
using HsR.UserService.Client.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace AssetsFlowWeb.API.Controllers
{
    [Route("hsr-api/v{version:apiVersion}/[controller]")]
    [ApiVersion("1.0")]
    [ApiController]
    [ResponseCache(Duration = 1800, Location = ResponseCacheLocation.Client)]
    public class InfoController : HsRControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IUserServiceClient _identityService;

        public InfoController(
            IWebHostEnvironment env,
            ILogger<InfoController> logger,
            IUserServiceClient identityService)
            : base(logger)
        {
            _env = env;
            _identityService = identityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetInfo()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var infoAttr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
            var versionRaw = infoAttr?.InformationalVersion ?? "unknown";

            // Strip build metadata after '+'
            var appVersion = versionRaw.Split('+')[0];

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
                try
                {
                    commitHash = GitWrappers.RunGitCommand("rev-parse --short HEAD");
                }
                catch { commitHash = "unknown"; }

                if (string.IsNullOrWhiteSpace(appVersion))
                {
                    try
                    {
                        appVersion = GitWrappers.RunGitCommand("describe --tags --abbrev=0");
                    }
                    catch { appVersion = "dev-unknown"; }
                }
            }

            // Fetch IdentityService version via gRPC
            string identityServiceVersion;
            try
            {
                identityServiceVersion = await _identityService.GetServiceVersionAsync();
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to get IdentityService version");
                identityServiceVersion = "unknown";
            }

            return Ok(new
            {
                beVersion = appVersion ?? "unknown",
                buildTimeStamp = buildTimeStamp ?? "unknown",
                gitCommit = commitHash ?? "unknown",
                identityServiceVersion
            });
        }
    }
}
