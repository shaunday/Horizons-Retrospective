using HsR.Journal.DataContext;
using HsR.Journal.Seeder;
using Microsoft.Extensions.DependencyInjection;

namespace HsR.Journal.DataSeeder
{
    public static class DatabaseSeederExtensions
    {
        public static async Task SeedDatabaseAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<TradingJournalDataContext>();
            await DatabaseSeeder.SeedAsync(dbContext);
        }
    }
}
