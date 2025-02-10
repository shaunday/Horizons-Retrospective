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

                // Add CORS services
                builder.Services.AddCors(options =>
                {
                    options.AddPolicy("AllowReactApp", policy =>
                    {
                        policy.WithOrigins("http://localhost:5173") // Allow requests from your React app
                              .AllowAnyMethod()                   // Allow all HTTP methods (GET, POST, etc.)
                              .AllowAnyHeader();                  // Allow all headers
                    });
                });

                builder.Services.AddScoped<DatabaseSeeder>();
            }
        }
    }

}
