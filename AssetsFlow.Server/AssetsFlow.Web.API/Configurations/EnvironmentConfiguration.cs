using HsR.Journal.Seeder;
using Microsoft.IdentityModel.Logging;

namespace HsR.Web.API.Configurations
{
    internal static class EnvironmentConfiguration
    {
        internal static void ConfigureForEnvironment(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;

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

                if (!string.IsNullOrEmpty(frontendUrl))
                {
                    builder.Services.AddCors(options =>
                    {
                        options.AddPolicy("AllowReactApp", policy =>
                        {
                            policy.WithOrigins(frontendUrl)
                                  .AllowAnyMethod()
                                  .AllowAnyHeader();
                        });
                    });
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