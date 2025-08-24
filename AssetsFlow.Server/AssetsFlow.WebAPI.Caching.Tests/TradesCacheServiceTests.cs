using AssetsFlowWeb.Services.Models.Journal;
using AutoMapper;
using HsR.Common.Services.Caching;
using HsR.Journal.DataContext;
using HsR.Journal.Entities.TradeJournal;
using HsR.Web.API.Services;
using HsR.Web.API.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace HsR.Web.API.Caching.Tests
{
    public class TradesCacheServiceTests
    {
        private IMemoryCache CreateMemoryCache(int? sizeLimit = null)
        {
            return new MemoryCache(new MemoryCacheOptions { SizeLimit = sizeLimit });
        }

        private (TradesCacheService service, IMemoryCache cache, Mock<IServiceProvider> sp, Mock<IMapper> mapper, Mock<ILogger<TradesCacheService>> logger, Mock<IConfigurationService> config)
            CreateService(int? sizeLimit = null)
        {
            var cache = CreateMemoryCache(sizeLimit);
            var sp = new Mock<IServiceProvider>();
            var mapper = new Mock<IMapper>();
            var logger = new Mock<ILogger<TradesCacheService>>();
            var config = new Mock<IConfigurationService>();
            config.SetupGet(c => c.Cache).Returns(new CacheSettings
            {
                CacheDurationMinutes = 5,
                PreloadPageCount = 1,
                SizeLimit = sizeLimit ?? 100,
                MaxConcurrentUsers = 10,
                LoadWaitTimeoutSeconds = 1,
                CleanupInactiveUsersThresholdHours = 1,
                CleanupIntervalMinutes = 10
            });
            var configMock = new Mock<IConfigurationService>();
            configMock.Setup(c => c.Cache.MaxConcurrentUsers).Returns(100);
            configMock.Setup(c => c.Cache.CleanupInactiveUsersThresholdHours).Returns(2);

            var service = new TradesCacheService(
                cache,
                sp.Object,
                mapper.Object,
                logger.Object,
                configMock.Object);

            return (service, cache, sp, mapper, logger, config);
        }

        [Fact]
        public async Task GetCachedTrades_Returns_Null_If_Not_Present()
        {
            var (service, _, _, _, _, _) = CreateService();
            var userId = Guid.NewGuid();
            var result = await service.GetCachedTrades(userId, 1, 10);
            Assert.Null(result);
        }

        [Fact]
        public async Task GetCachedTrades_Returns_Value_If_Present()
        {
            var (service, cache, _, _, _, _) = CreateService();
            var userId = Guid.NewGuid();
            var key = service.GetType().GetMethod("GetCacheKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.Invoke(service, new object[] { userId, 1, 10 }) as string;

            var expected = new List<TradeCompositeModel> { new TradeCompositeModel() };
            cache.Set(key!, expected, new MemoryCacheEntryOptions().SetSize(1));

            var result = await service.GetCachedTrades(userId, 1, 10);
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void InvalidateAndReload_Creates_Task()
        {
            var (service, _, _, _, _, _) = CreateService();
            var userId = Guid.NewGuid();

            service.InvalidateAndReload(userId);

            var loadTasksField = typeof(CacheServiceBase<Guid, IEnumerable<TradeCompositeModel>>)
                .GetField("_loadTasks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var tasksDict = (ConcurrentDictionary<string, Task>?)loadTasksField?.GetValue(service);

            Assert.NotNull(tasksDict);
            Assert.Contains(tasksDict!.Keys, k => k.Contains(userId.ToString()));
        }

        [Fact]
        public void CleanupInactive_Removes_CompletedTasks()
        {
            var (service, _, _, _, _, _) = CreateService();
            var userId = Guid.NewGuid();

            var loadTasksField = typeof(CacheServiceBase<Guid, IEnumerable<TradeCompositeModel>>)
                .GetField("_loadTasks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var tasksDict = (ConcurrentDictionary<string, Task>?)loadTasksField?.GetValue(service);
            Assert.NotNull(tasksDict);

            var cacheKeyField = typeof(CacheServiceBase<Guid, IEnumerable<TradeCompositeModel>>)
                .GetMethod("GetCacheKey", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            string key = (string)cacheKeyField!.Invoke(service, new object[] { userId })!;

            tasksDict![key] = Task.CompletedTask;

            service.CleanupInactive(TimeSpan.Zero);

            Assert.False(tasksDict!.ContainsKey(key));
        }

        [Fact]
        public void GetCachedTotalCount_Returns_Correct_Value()
        {
            var (service, cache, _, _, _, _) = CreateService();
            var userId = Guid.NewGuid();
            var key = $"trades_total_count_{userId}";
            cache.Set(key, 42);
            Assert.Equal(42, service.GetCachedTotalCount(userId));
        }
    }
}
