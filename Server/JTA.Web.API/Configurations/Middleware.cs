using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace JTA.Web.API.Configurations
{
    internal static class Middleware
    {
        internal static void ConfigureMiddleware(WebApplication app)
        {
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Use Swagger for API documentation in development.
                app.UseSwagger();
                app.UseSwaggerUI();

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

            // Enforce HTTPS redirection.
            app.UseHttpsRedirection();

            // Enable routing for the application.
            app.UseRouting();

            // Apply authorization middleware.
            app.UseAuthorization();

            // Map controllers for request handling.
            app.MapControllers();

        }
    }
}
