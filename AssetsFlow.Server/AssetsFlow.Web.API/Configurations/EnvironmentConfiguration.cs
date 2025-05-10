using HsR.Journal.Seeder;
using Microsoft.IdentityModel.Logging;
using Serilog;

namespace HsR.Web.API.Configurations
{
    internal static class EnvironmentConfiguration
    {
        internal static void ConfigureForEnvironment(this WebApplicationBuilder builder)
        {
            // Log the environment (Dev or Prod)
            Log.Information("Configuring for environment: {Environment}", builder.Environment.EnvironmentName);

            string? corsOrigin = null;

            if (builder.Environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
                corsOrigin = "http://localhost:5173";

                builder.Services.AddScoped<DatabaseSeeder>();
            }
            else
            {
                corsOrigin = Environment.GetEnvironmentVariable("FRONTEND_URL");

                if (!string.IsNullOrEmpty(corsOrigin))
                {
                    Log.Information("CORS policy set for frontend URL: {FrontendUrl}", corsOrigin);
                }
                else
                {
                    Log.Warning("FRONTEND_URL environment variable is not set. CORS policy will not be applied for production.");
                }

                builder.WebHost.UseUrls(
                    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
                        ? "http://0.0.0.0:80"
                        : "https://localhost:5000");
            }

            if (!string.IsNullOrEmpty(corsOrigin))
            {
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowReactApp", policy =>
                    {
                        policy.WithOrigins(corsOrigin)
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
                });
            }
        }
    }
}