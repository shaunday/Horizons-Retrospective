using DayJT.Journal.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace JTA.Web.API.DesignTime;

/// <summary>
/// Factory for creating instances of <see cref="TradingJournalDataContext"/> at design time.
/// </summary>
public class TradingJournalContextFactory : IDesignTimeDbContextFactory<TradingJournalDataContext>
{
    private const string AdminConnectionString = "JTA_ADMIN_CONNECTION_STRING";

    /// <summary>
    /// Creates a new instance of <see cref="TradingJournalDataContext"/> using the provided arguments.
    /// </summary>
    /// <param name="args">Arguments for creating the context.</param>
    /// <returns>A new instance of <see cref="TradingJournalDataContext"/>.</returns>
    /// <exception cref="ApplicationException">Thrown if the connection string is not set.</exception>
    public TradingJournalDataContext CreateDbContext(string[] args)
    {
        var connectionString = Environment.GetEnvironmentVariable(AdminConnectionString);
        if (string.IsNullOrEmpty(connectionString))
        {
            throw new ApplicationException(
                $"Please set the environment variable {AdminConnectionString}");
        }

        var options = new DbContextOptionsBuilder<TradingJournalDataContext>()
            .UseNpgsql(connectionString, sqlOptions =>
            {
                sqlOptions.MigrationsAssembly(typeof(TradingJournalDataContext).Assembly.FullName);
            })
            .Options;
        return new TradingJournalDataContext(options);
    }
}
