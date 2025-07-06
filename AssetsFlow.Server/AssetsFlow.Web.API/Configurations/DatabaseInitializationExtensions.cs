using HsR.Journal.DataContext;
using HsR.Journal.DataSeeder;
using HsR.Common.AspNet;
using Microsoft.EntityFrameworkCore;

namespace AssetsFlowWeb.API.Configurations
{
    public static class DatabaseInitializationExtensions
    {
        public static async Task EnsureAssetsFlowDatabaseAndSeedAsync(this WebApplication app)
        {
            // Ensure database is created using the generic method
            await app.EnsureDatabaseCreatedAsync<TradingJournalDataContext>();

            // Seed database only in development
            if (app.Environment.IsDevelopment())
            {
                await app.Services.FlushDbAndSeedDemoAsync();
            }
        }
    }
} 