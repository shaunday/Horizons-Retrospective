using DayJT.Journal.DataContext.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DayJT.Journal.Test
{
    [TestClass]
    public sealed class DatabaseTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var builder = new DbContextOptionsBuilder<TradingJournalDataContext>();
            builder.UseNpgsql(configuration.GetConnectionString("DayJTestDatabase"));

            using (var context = new TradingJournalDataContext(builder.Options))
            {
                //context.Database.EnsureDeleted();
                //context.Database.EnsureCreated();
            }
        }
    }
}
