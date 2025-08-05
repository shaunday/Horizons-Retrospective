using DotNetEnv;
using HsR.UserService.Contracts;
using HsR.UserService.Host.Configurations;
using HsR.UserService.Infrastructure;
using Microsoft.AspNetCore.Server.Kestrel.Core;
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
            string baseUrl = UserServiceEx.GetUserServiceUrl();

            builder.WebHost.UseUrls(baseUrl);

            if (Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true")
            {
                builder.WebHost.ConfigureKestrel(options =>
                {
                    options.ListenAnyIP(80, listenOptions =>
                    {
                        listenOptions.Protocols = HttpProtocols.Http2; // Enforce HTTP/2 for gRPC
                    });
                });
            }

            return builder;
        }

        public static string GetUserServiceConnectionString(this WebApplicationBuilder builder)
        {
            bool isDev = builder.Environment.IsDevelopment();
            return UsersDbConnectionsWrapper.GetConnectionStringByEnv(isDev);
        }
    }
} 