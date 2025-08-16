using HsR.Journal.DataContext;
using HsR.Journal.Infrastructure;
using HsR.Journal.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql.EntityFrameworkCore.PostgreSQL.Infrastructure;
using System;

namespace HsR.Journal.Infrastructure
{
    public static class ServiceConfiguration
    {
        public static IServiceCollection ConfigureTradingJournalDbContext(this IServiceCollection services, bool isDev)
        {
            string connectionString = DbConnectionsWrapper.GetConnectionStringByEnv(isDev);
            var migrationsAssembly = typeof(ServiceConfiguration).Assembly.FullName;

            services.AddDbContextPool<TradingJournalDataContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.MigrationsAssembly(migrationsAssembly);
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
