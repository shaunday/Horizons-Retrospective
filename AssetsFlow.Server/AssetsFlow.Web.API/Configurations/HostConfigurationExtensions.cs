using AssetsFlowWeb.API.Configurations;
using HsR.Infrastructure;
using DotNetEnv;
using Serilog;
using HsR.Journal.Infrastructure;

namespace AssetsFlowWeb.API.Configurations
{
    public static class HostConfigurationExtensions
    {
        public static WebApplicationBuilder ConfigureAssetsFlowHost(this WebApplicationBuilder builder)
        {
            // Load shared environment variables
            DotNetEnv.Env.Load(".env.AssetsFlow");

            bool isDev = builder.Environment.IsDevelopment();
            LoggingConfiguration.ConfigureLogging(isDev);
            builder.Host.UseSerilog();

            return builder;
        }

        public static string GetAssetsFlowConnectionString(this WebApplicationBuilder builder)
        {
            bool isDev = builder.Environment.IsDevelopment();
            return DbConnectionsWrapper.GetConnectionStringByEnv(isDev);
        }
    }
} 