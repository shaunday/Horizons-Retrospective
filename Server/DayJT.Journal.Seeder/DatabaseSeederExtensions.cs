using DayJT.Journal.Repository;
using DayJTrading.Journal.Seeder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DayJT.Journal.DataSeeder
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
