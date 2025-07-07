using HsR.Web.API.Settings;
using HsR.Web.API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class CacheServicesExtensions
{
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