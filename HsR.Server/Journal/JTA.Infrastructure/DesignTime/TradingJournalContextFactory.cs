using HsR.Journal.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HsR.Infrastructure;

public class TradingJournalContextFactory : IDesignTimeDbContextFactory<TradingJournalDataContext>
{
    public static string AdminConnectionString = "HsR_Journal_ADMIN_CONNECTION";

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
