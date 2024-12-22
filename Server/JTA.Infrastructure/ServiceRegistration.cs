using JTA.Infrastructure.JTA.Journal.Repository.CompiledModels;
using JTA.Journal.Repository;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;

namespace JTA.Infrastructure
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
        {
            ConfigureDbContext(services, connectionString, null);
            return services;
        }

        public static IServiceCollection AddInfrastructureWithLogging(this IServiceCollection services, string connectionString, IWebHostEnvironment environment)
        {
            ConfigureDbContext(services, connectionString, environment);
            return services;
        }

        private static void ConfigureDbContext(IServiceCollection services, string connectionString, IWebHostEnvironment? environment)
        {
            services.AddDbContextPool<TradingJournalDataContext>(options =>
            {
                options.UseNpgsql(connectionString, npgsqlOptions =>
                {
                    npgsqlOptions.EnableRetryOnFailure();
                });

                if (environment != null)
                {
                    if (environment.IsDevelopment())
                    {
                        options.LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging();
                    }
                    else
                    {
                        options.UseModel(TradingJournalDataContextModel.Instance);   
                    }
                }
            });
        }
    }

}
