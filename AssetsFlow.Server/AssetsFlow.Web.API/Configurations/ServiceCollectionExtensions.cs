using HsR.Web.API.Configuration;
using HsR.Web.API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AssetsFlowWeb.API.Configurations
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddConfigurationServices(this IServiceCollection services, IConfiguration configuration)
        {
            // Configure settings
            services.Configure<PaginationSettings>(configuration.GetSection("PaginationSettings"));
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));

            // Register services
            services.AddSingleton<IConfigurationService, ConfigurationService>();

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