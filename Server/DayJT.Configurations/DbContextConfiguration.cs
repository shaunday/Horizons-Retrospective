using DayJT.Journal.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace DayJT.Configurations
{
    public static class DbContextConfiguration
    {
        public static void AddTradingJournalDbContext(this IServiceCollection services, IConfiguration configuration, IHostEnvironment environment)
        {
            services.AddDbContextPool<TradingJournalDataContext>(options =>
            {
                options.UseNpgsql(configuration.GetConnectionString("DayJTradingDbKey"), npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure();
                });

                if (environment.IsDevelopment())
                {
                    options.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging(); // Enable logging in Development
                }
            });
        }
    }
}
