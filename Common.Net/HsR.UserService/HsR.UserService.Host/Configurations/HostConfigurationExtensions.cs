using HsR.UserService.Host.Configurations;
using HsR.UserService.Infrastructure;
using DotNetEnv;
using Serilog;

namespace HsR.UserService.Host.Configurations
{
    public static class HostConfigurationExtensions
    {
        public static WebApplicationBuilder ConfigureUserServiceHost(this WebApplicationBuilder builder)
        {
            // Load shared environment variables
            DotNetEnv.Env.Load(".env.AssetsFlow");

            // Configure Serilog
            LoggingConfiguration.ConfigureLogging(builder.Environment.IsDevelopment());
            builder.Host.UseSerilog();

            // Configure URLs based on environment
            var isContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
            var userServicePort = isContainer ? (Environment.GetEnvironmentVariable("USER_SERVICE_PORT") ?? "7001") : "7001";
            builder.WebHost.UseUrls(
                isContainer
                    ? $"http://0.0.0.0:{userServicePort}"
                    : $"https://localhost:{userServicePort}");

            return builder;
        }

        public static string GetUserServiceConnectionString(this WebApplicationBuilder builder)
        {
            bool isDev = builder.Environment.IsDevelopment();
            return DbConnectionsWrapper.GetConnectionStringByEnv(isDev);
        }
    }
} 