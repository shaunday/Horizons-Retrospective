using AssetsFlowWeb.Services.Models;
using AssetsFlowWeb.Services.Models.Journal;
using HsR.Common.Services.Caching;
using HsR.Journal.Entities;
using HsR.UserService.Protos;
using HsR.Web.API.Services;
using HsR.Web.API.Settings;
using Microsoft.Extensions.Caching.Memory;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCacheServices(this IServiceCollection services, IConfiguration configuration)
    {
        var cacheSettings = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? new CacheSettings();

        services.AddMemoryCache(options =>
        {
            options.SizeLimit = cacheSettings.SizeLimit;
        });

        // Register cache services
        services.AddSingleton<ICacheService<Guid, IEnumerable<TradeCompositeModel>>, TradesCacheService>();
        services.AddSingleton<ICacheService<Guid, UserDataDTO>, UserDataCacheService>();

        // Register cleanup background services
        services.AddHostedService<CacheCleanupService<Guid, List<TradeCompositeModel>>>(provider =>
        {
            var cache = provider.GetRequiredService<ICacheService<Guid, List<TradeCompositeModel>>>();
            var logger = provider.GetRequiredService<ILogger<CacheCleanupService<Guid, List<TradeCompositeModel>>>>();
            return new CacheCleanupService<Guid, List<TradeCompositeModel>>(
                cache,
                logger,
                TimeSpan.FromMinutes(cacheSettings.CleanupIntervalMinutes),
                TimeSpan.FromHours(cacheSettings.CleanupInactiveUsersThresholdHours)
            );
        });

        services.AddHostedService<CacheCleanupService<Guid, UserDto>>(provider =>
        {
            var cache = provider.GetRequiredService<ICacheService<Guid, UserDto>>();
            var logger = provider.GetRequiredService<ILogger<CacheCleanupService<Guid, UserDto>>>();
            return new CacheCleanupService<Guid, UserDto>(
                cache,
                logger,
                TimeSpan.FromMinutes(cacheSettings.CleanupIntervalMinutes),
                TimeSpan.FromHours(cacheSettings.CleanupInactiveUsersThresholdHours)
            );
        });

        return services;
    }
}
