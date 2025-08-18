using HsR.Journal.DataContext;
using HsR.Journal.Seeder;
using Microsoft.Extensions.DependencyInjection;

namespace HsR.Journal.DataSeeder
{
    public static class DatabaseSeederExtensions
    {
        public static async Task FlushDbAndSeedDemoAsync(this IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var seeder = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
            await seeder.FlushDbAndSeedDemoAsync();
        }
    }
}
