using HsR.Journal.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace HsR.Infrastructure;

public class TradingJournalContextFactory : IDesignTimeDbContextFactory<TradingJournalDataContext>
{
    public static string DbConnectionString = "HsR_AssetsFlow_Connection_String";

    public TradingJournalDataContext CreateDbContext(string[] args)
    {
        DotNetEnv.Env.Load();
        var connectionString = Environment.GetEnvironmentVariable(DbConnectionString);
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ApplicationException(
                $"Please set the environment variable {DbConnectionString}");
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
