using JTA.Journal.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JTA.Infrastructure;

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
        var migrationsAssembly = typeof(TradingJournalContextFactory).Assembly.GetName().Name;

        optionsBuilder.UseNpgsql(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(migrationsAssembly);
            });

        return new TradingJournalDataContext(optionsBuilder.Options);
    }
}