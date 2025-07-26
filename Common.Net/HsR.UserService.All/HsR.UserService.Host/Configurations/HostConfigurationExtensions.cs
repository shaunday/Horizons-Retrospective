using HsR.UserService.Host.Configurations;
using HsR.UserService.Infrastructure;
using DotNetEnv;
using Serilog;
using HsR.UserService.Contracts;

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
            string baseUrl = UserServiceEx.GetUserServiceUrl();

            builder.WebHost.UseUrls(baseUrl);
            return builder;
        }

        public static string GetUserServiceConnectionString(this WebApplicationBuilder builder)
        {
            bool isDev = builder.Environment.IsDevelopment();
            return UsersDbConnectionsWrapper.GetConnectionStringByEnv(isDev);
        }
    }
} 