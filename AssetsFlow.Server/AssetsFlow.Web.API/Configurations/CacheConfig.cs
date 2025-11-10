using AssetsFlowWeb.Services.Models;
using AssetsFlowWeb.Services.Models.Journal;
using HsR.Common.Services.Caching;
using HsR.Journal.Entities;
using HsR.UserService.Protos;
using HsR.Web.API.Services;
using HsR.Web.API.Settings;
using Microsoft.Extensions.Caching.Memory;
using Serilog;

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
        services.AddSingleton<ITradesCacheService, TradesCacheService>();
        services.AddSingleton<ICacheService<Guid, IEnumerable<TradeCompositeModel>>>(provider => provider.GetRequiredService<ITradesCacheService>());
        services.AddSingleton<ICacheService<Guid, UserDataDTO>, UserDataCacheService>();

        // Register cleanup background services
        services.AddHostedService(provider =>
        {
            var cache = provider.GetRequiredService<ICacheService<Guid, IEnumerable<TradeCompositeModel>>>();
            var logger = Log.ForContext("SourceContext", "CacheCleanupService");
            return new CacheCleanupService<Guid, IEnumerable<TradeCompositeModel>>(
                cache,
                logger,
                TimeSpan.FromMinutes(cacheSettings.CleanupIntervalMinutes),
                TimeSpan.FromHours(cacheSettings.CleanupInactiveUsersThresholdHours)
            );
        });

        services.AddHostedService(provider =>
        {
            var cache = provider.GetRequiredService<ICacheService<Guid, UserDataDTO>>();
            var logger = Log.ForContext("SourceContext", "CacheCleanupService");
            return new CacheCleanupService<Guid, UserDataDTO>(
                cache,
                logger,
                TimeSpan.FromMinutes(cacheSettings.CleanupIntervalMinutes),
                TimeSpan.FromHours(cacheSettings.CleanupInactiveUsersThresholdHours)
            );
        });

        return services;
    }
}
