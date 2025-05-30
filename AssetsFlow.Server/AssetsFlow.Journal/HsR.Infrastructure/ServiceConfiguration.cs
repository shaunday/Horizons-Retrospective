using HsR.Journal.DataContext;
using HsR.Journal.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
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
                    npgsqlOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                });

                if (isDev)
                {
                    options.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
                }
            });
            return services;
        }
    }
}
