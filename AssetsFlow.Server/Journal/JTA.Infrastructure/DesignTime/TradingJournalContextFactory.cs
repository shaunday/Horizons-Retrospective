using HsR.Journal.DataContext;
using HsR.Journal.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HsR.Infrastructure;

public class TradingJournalContextFactory : IDesignTimeDbContextFactory<TradingJournalDataContext>
{
    public TradingJournalDataContext CreateDbContext(string[] args)
    {
        var connectionString = DbConnectionInfo.GetConnectionStringByEnv();
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ApplicationException($"Please set  environment variables");
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
