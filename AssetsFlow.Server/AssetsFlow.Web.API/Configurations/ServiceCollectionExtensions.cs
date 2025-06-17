using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HsR.Web.API.Services;
using HsR.Web.API.Configuration;

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
            services.AddScoped<IConfigurationService, ConfigurationService>();
            
            return services;
        }

        public static IServiceCollection AddCacheServices(this IServiceCollection services)
        {
            services.AddMemoryCache();
            services.AddScoped<ITradesCacheService, TradesCacheService>();
            
            return services;
        }
    }
} 