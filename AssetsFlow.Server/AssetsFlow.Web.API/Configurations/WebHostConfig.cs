using Microsoft.AspNetCore.Hosting;

namespace AssetsFlowWeb.API.Configurations
{
    public static class WebHostConfig
    {
        public static void ConfigureUrls(IWebHostBuilder webHostBuilder)
        {
            webHostBuilder.UseUrls(
                Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
                    ? "http://0.0.0.0:80"
                    : "http://localhost:5000");
        }
    }
} 