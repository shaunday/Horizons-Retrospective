using HsR.Journal.DataContext;
using HsR.Journal.DataSeeder;
using HsR.Journal.Seeder;
using Microsoft.EntityFrameworkCore;

namespace AssetsFlowWeb.API.Configurations
{
    public static class DbAndSeederConfigs
    {
        internal static IServiceCollection RegisterDbSeeder(this IServiceCollection services)
        {
            services.AddScoped<DatabaseSeeder>();
            return services;
        }

        internal static async Task InitDB(this WebApplication app, bool isDev)
        {
            if (isDev)
            {
               // await app.Services.FlushDbAndSeedDemoAsync();
            }
            else
            {
                using var scope = app.Services.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<TradingJournalDataContext>();
                await db.Database.MigrateAsync();  // Apply migrations on startup
            }

        }
    }
}
