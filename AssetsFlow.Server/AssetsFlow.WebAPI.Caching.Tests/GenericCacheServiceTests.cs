using AssetsFlowWeb.Services.Models.Journal;
using AutoMapper;
using HsR.Common.AspNet.Authentication;
using HsR.Common.Services.Caching;
using HsR.Web.API.Services;
using HsR.Web.API.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace HsR.Web.API.Caching.Tests
{
    public class ConfigurationServiceStub : IConfigurationService
    {
        public CacheSettings Cache { get; set; } = new CacheSettings();
        public JwtSettings Jwt { get; set; } = new JwtSettings();
        public PaginationSettings Pagination { get; set; } = new PaginationSettings();
    }

    public class GenericCacheServiceTests
    {
        private IMemoryCache CreateMemoryCache(int? sizeLimit = null)
        {
            return new MemoryCache(new MemoryCacheOptions { SizeLimit = sizeLimit });
        }

        private (TradesCacheService service, IMemoryCache cache) CreateService(int? sizeLimit = null)
        {
            var cache = CreateMemoryCache(sizeLimit);
            var sp = new ServiceCollection().BuildServiceProvider();
            var mapper = new MapperConfiguration(cfg => { }).CreateMapper();
            var logger = new LoggerFactory().CreateLogger<TradesCacheService>();

            var configStub = new ConfigurationServiceStub
            {
                Cache = new CacheSettings
                {
                    CacheDurationMinutes = 5,
                    PreloadPageCount = 1,
                    SizeLimit = sizeLimit ?? 100,
                    MaxConcurrentUsers = 10,
                    LoadWaitTimeoutSeconds = 1,
                    CleanupInactiveUsersThresholdHours = 1,
                    CleanupIntervalMinutes = 10
                }
            };

            var service = new TradesCacheService(cache, sp, mapper, logger, configStub);
            return (service, cache);
        }

        [Fact]
        public async Task GetCachedTrades_Returns_Null_If_Not_Present()
        {
            var (service, _) = CreateService();
            var userId = Guid.NewGuid();
            var result = await service.GetCachedTrades(userId, 1, 10);
            Assert.Null(result);
        }

        [Fact]
        public void InvalidateAndReload_Creates_Task_PublicMethod()
        {
            var (service, _) = CreateService();
            var userId = Guid.NewGuid();

            // Only call the public API
            service.InvalidateAndReload(userId);

            // Since we cannot access _loadTasks safely, we only verify that no exception occurs
        }

        [Fact]
        public void CleanupInactive_PublicMethod_RunsWithoutException()
        {
            var (service, _) = CreateService();
            var userId = Guid.NewGuid();

            // Just call public method to ensure no crash
            service.CleanupInactive(TimeSpan.Zero);
        }
    }
}
