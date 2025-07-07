using HsR.Web.API.Settings;
using HsR.Web.API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using HsR.UserService.Client.Extensions;
using HsR.Infrastructure;

namespace HsR.Web.API.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddAssetsFlowServices(this IServiceCollection services, IConfiguration configuration, string connectionString, bool isDev, IHostEnvironment environment)
        {
            return services
                .AddConfigurationServices(configuration)
                .AddCacheServices(configuration)
                .AddUserServiceClient(configuration, environment);
        }

        public static IServiceCollection AddConfigurationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure settings
            services.Configure<PaginationSettings>(configuration.GetSection("PaginationSettings"));
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            // Register services
            services.AddSingleton<IConfigurationService, ConfigurationService>();
            services.AddSingleton<IJwtService, JwtService>();

            return services;
        }

        public static IServiceCollection AddCacheServices(this IServiceCollection services, IConfiguration configuration)
        {
            var cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? new CacheSettings();
            services.AddMemoryCache(options =>
            {
                options.SizeLimit = cacheSettings.SizeLimit;
            });
            services.AddSingleton<ITradesCacheService, TradesCacheService>();
            services.AddHostedService<CacheCleanupService>(provider =>
                new CacheCleanupService(
                    provider.GetRequiredService<ITradesCacheService>(),
                    provider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<CacheCleanupService>>(),
                    provider.GetRequiredService<IConfigurationService>()
                )
            );
            return services;
        }
    }
} 