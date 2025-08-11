using Microsoft.IdentityModel.Logging;
using HsR.Journal.Seeder;
using Serilog;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace AssetsFlowWeb.API.Configurations
{
    internal static class CorsConfig
    {
        public const string AllowReactAppPolicyName = "AllowReactApp";

        internal static IServiceCollection ConfigureCors(this IServiceCollection services, bool isDev)
        {
            string? corsOrigin = null;
            if (isDev)
            {
                corsOrigin = "http://localhost:5173";
            }
            else
            {
                corsOrigin = "https://hsr.mywebthings.xyz";
                if (!string.IsNullOrEmpty(corsOrigin))
                {
                    Log.Information("CORS policy set for frontend URL: {FrontendUrl}", corsOrigin);
                }
            }
            if (!string.IsNullOrEmpty(corsOrigin))
            {
                services.AddCors(options =>
                {
                    options.AddPolicy(AllowReactAppPolicyName, policy =>
                    {
                        policy.WithOrigins(corsOrigin)
                              .AllowAnyMethod()
                              .AllowAnyHeader()
                              .AllowCredentials();
                    });
                });
            }
            return services;
        }
    }
}