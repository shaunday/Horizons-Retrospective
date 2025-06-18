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

        public static IServiceCollection AddCacheServices(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddSingleton<ITradesCacheService, TradesCacheService>();

            return services;
        }
    }
} 