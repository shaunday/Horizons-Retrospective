using Microsoft.IdentityModel.Logging;

namespace HsR.Web.API.Configurations
{
    public static class EnvironmentConfiguration
    {
        public static void ConfigureForEnvironment(this WebApplicationBuilder builder)
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
            }
        }
    }

}
