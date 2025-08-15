using HsR.Middleware;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AssetsFlowWeb.API.Configurations
{
    internal static class Middleware
    {
        internal static void ConfigureMiddleware(WebApplication app)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                //app.UseSwagger();
                //app.UseSwaggerUI();

                // Enable the developer exception page for detailed error messages.
                app.UseDeveloperExceptionPage();

                // Custom middleware to handle database exceptions.
                app.Use(async (context, next) =>
                {
                    try
                    {
                        await next.Invoke();
                    }
                    catch (Exception ex) when (ex is DbUpdateException || ex is SqlException)
                    {
                        Log.Logger.Error("Database exception: " + ex);
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        await context.Response.WriteAsync("A database error occurred.");
                    }
                });
            }

            app.UseRouting();

            // ✅ Apply CORS after routing, before endpoints
            app.UseCors(CorsConfig.AllowReactAppPolicyName);

            // Apply authentication and authorization middleware.
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseMiddleware<UserScopeMiddleware>();

            app.MapControllers();
        }
    }
}