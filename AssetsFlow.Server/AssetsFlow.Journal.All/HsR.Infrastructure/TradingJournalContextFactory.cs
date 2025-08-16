using HsR.Journal.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HsR.Journal.Infrastructure;

public class TradingJournalContextFactory : IDesignTimeDbContextFactory<TradingJournalDataContext>
{
    public TradingJournalDataContext CreateDbContext(string[] args)
    {
        var connectionString = DbConnectionsWrapper.GetConnectionStringByEnv(false); //used for migration scripts, so set to production for now
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
