using HsR.Journal.DataContext;
using HsR.Journal.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace HsR.Infrastructure
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection ConfigureTradingJournalDbContext(this IServiceCollection services, string connectionString, bool isDev)
        {
            services.AddDbContextPool<TradingJournalDataContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure();
                });

                if (isDev)
                {
                    options.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
                }
                else
                {
                    //options.UseModel(TradingJournalDataContextModel.Instance);   //todo production
                }
            });
            return services;
        }
    }
}
