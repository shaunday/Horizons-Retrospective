using DayJT.Journal.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace DayJT.Infrastructure;

public class TradingJournalContextFactory : IDesignTimeDbContextFactory<TradingJournalDataContext>
{
    private const string AdminConnectionString = "JTA_ADMIN_CONNECTION_STRING";

    public TradingJournalDataContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable(AdminConnectionString);
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ApplicationException(
                $"Please set the environment variable {AdminConnectionString}");
        }

        var optionsBuilder = new DbContextOptionsBuilder<TradingJournalDataContext>();
        optionsBuilder.UseNpgsql(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly("JTA.Infrastructure");
            });

        return new TradingJournalDataContext(optionsBuilder.Options);
    }
}
