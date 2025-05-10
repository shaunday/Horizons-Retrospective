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

            if (builder.Environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;

                // Log for dev environment
                Log.Information("Development environment detected. Enabling CORS for localhost:5173");

                // Add CORS for local development
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowReactApp", policy =>
                    {
                        policy.WithOrigins("http://localhost:5173")
                              .AllowAnyMethod()
                              .AllowAnyHeader();
                    });
                });

                builder.Services.AddScoped<DatabaseSeeder>();
            }
            else
            {
                var frontendUrl = Environment.GetEnvironmentVariable("FRONTEND_URL");

                // Log frontend URL (production URL or null)
                Log.Information("Production environment detected. FRONTEND_URL: {FrontendUrl}", frontendUrl ?? "Not set");

                if (!string.IsNullOrEmpty(frontendUrl))
                {
                    builder.Services.AddCors(options =>
                    {
                        options.AddPolicy("AllowReactApp", policy =>
                        {
                            Log.Information("CORS policy set for frontend URL: {FrontendUrl}", frontendUrl);
                            policy.WithOrigins(frontendUrl)
                                  .AllowAnyMethod()
                                  .AllowAnyHeader();
                        });
                    });
                }
                else
                {
                    Log.Warning("FRONTEND_URL environment variable is not set. CORS policy will not be applied for production.");
                }

                builder.Services.AddScoped<DatabaseSeeder>();

                builder.WebHost.UseUrls(
                    Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true"
                        ? "http://0.0.0.0:80"
                        : "https://localhost:5000");
            }
        }
    }
}