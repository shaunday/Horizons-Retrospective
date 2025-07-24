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
            // Configure Serilog
            LoggingConfiguration.ConfigureLogging(builder.Environment.IsDevelopment());
            builder.Host.UseSerilog();

            // Determine the base URL based on environment
            string baseUrl;
            if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
            {
                baseUrl = "http://userservice:80";
            }
            else
            {
                var userServicePort = Environment.GetEnvironmentVariable("USER_SERVICE_PORT") ?? "7001";
                baseUrl = $"https://localhost:{userServicePort}";
            }
            builder.WebHost.UseUrls(baseUrl);

            return builder;
        }

        public static string GetUserServiceConnectionString(this WebApplicationBuilder builder)
        {
            bool isDev = builder.Environment.IsDevelopment();
            return DbConnectionsWrapper.GetConnectionStringByEnv(isDev);
        }
    }
} 