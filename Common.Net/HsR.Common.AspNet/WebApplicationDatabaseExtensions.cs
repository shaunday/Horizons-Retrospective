using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HsR.Common.AspNet
{
    public static class WebApplicationDatabaseExtensions
    {
        public static async Task<WebApplication> EnsureDatabaseCreatedAsync<TContext>(this WebApplication app)
            where TContext : DbContext
        {
            using var scope = app.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TContext>();
            await context.Database.EnsureCreatedAsync();
            return app;
        }
    }
} 