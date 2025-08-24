using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HsR.Common.Services.Caching
{
    public class CacheCleanupService<TId, TModel> : BackgroundService
    {
        private readonly ICacheService<TId, TModel> _cacheService;
        private readonly ILogger<CacheCleanupService<TId, TModel>> _logger;
        private readonly TimeSpan _cleanupInterval;
        private readonly TimeSpan _inactivityThreshold;

        public CacheCleanupService(
            ICacheService<TId, TModel> cacheService,
            ILogger<CacheCleanupService<TId, TModel>> logger,
            TimeSpan cleanupInterval,
            TimeSpan inactivityThreshold)
        {
            _cacheService = cacheService;
            _logger = logger;
            _cleanupInterval = cleanupInterval;
            _inactivityThreshold = inactivityThreshold;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _cacheService.CleanupInactive(_inactivityThreshold);
                    _logger.LogDebug("Cache cleanup completed for {CacheService}", typeof(TModel).Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during cache cleanup for {CacheService}", typeof(TModel).Name);
                }

                await Task.Delay(_cleanupInterval, stoppingToken);
            }
        }
    }
}
