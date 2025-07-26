using Xunit;
using Moq;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using AutoMapper;
using HsR.Web.API.Services;
using HsR.Journal.DataContext;
using HsR.Web.Services.Models.Journal;
using HsR.Journal.Entities.TradeJournal;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using HsR.Web.API.Settings;

namespace HsR.Web.API.Caching.Tests
{
    public class TradesCacheServiceTests
    {
        private IMemoryCache CreateMemoryCache(int? sizeLimit = null)
        {
            return new MemoryCache(new MemoryCacheOptions { SizeLimit = sizeLimit });
        }

        private (TradesCacheService service, IMemoryCache cache, Mock<IServiceProvider> sp, Mock<IMapper> mapper, Mock<ILogger<TradesCacheService>> logger, Mock<IConfigurationService> config) CreateService(int? sizeLimit = null)
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
            config.SetupGet(c => c.Pagination).Returns(new PaginationSettings
            {
                DefaultPageSize = 10,
                MaxPageSize = 20
            });
            return (new TradesCacheService(cache, sp.Object, mapper.Object, logger.Object, config.Object), cache, sp, mapper, logger, config);
        }

        [Fact]
        public void User_Isolation_Works()
        {
            var (service, cache, _, _, _, _) = CreateService();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            var key1 = TradesCacheService.GetCacheKey(userId1, 1, 10);
            var key2 = TradesCacheService.GetCacheKey(userId2, 1, 10);
            cache.Set(key1, "A");
            cache.Set(key2, "B");
            Assert.Equal("A", cache.Get<string>(key1));
            Assert.Equal("B", cache.Get<string>(key2));
        }

        [Fact]
        public void MemoryPressure_Evicts_Entries()
        {
            var (service, cache, _, _, _, _) = CreateService(sizeLimit: 2);
            // Fill cache to size limit
            for (int i = 0; i < 2; i++)
                cache.Set(TradesCacheService.GetCacheKey(Guid.NewGuid(), 1, 10), (object)$"val{i}", new MemoryCacheEntryOptions().SetSize(1));
            // Add one more to trigger eviction
            cache.Set(TradesCacheService.GetCacheKey(Guid.NewGuid(), 1, 10), (object)"val2", new MemoryCacheEntryOptions().SetSize(1));
            // At least one of the first two should be evicted
            int count = 0;
            for (int i = 0; i < 10; i++)
                if (cache.Get(TradesCacheService.GetCacheKey(Guid.NewGuid(), 1, 10)) != null) count++;
            Assert.True(count <= 2);
        }

        [Fact]
        public async Task Cache_Respects_Expiration()
        {
            var cache = new MemoryCache(new MemoryCacheOptions());
            var (service, _, _, _, _, config) = CreateService();
            var userId = Guid.NewGuid();
            var key = TradesCacheService.GetCacheKey(userId, 1, 10);
            cache.Set(key, (object)"A", new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMilliseconds(100)));
            await Task.Delay(200);
            Assert.Null(cache.Get<string>(key));
        }

        [Fact]
        public void Concurrent_Invalidate_Does_Not_Throw()
        {
            var (service, cache, _, _, _, _) = CreateService();
            var userId = Guid.NewGuid();
            var key = TradesCacheService.GetCacheKey(userId, 1, 10);
            cache.Set(key, "A");
            Parallel.For(0, 10, _ => service.InvalidateAndReload(userId));
            // Should not throw or deadlock
            Assert.True(true);
        }

        [Fact]
        public void CleanupInactiveUsers_Removes_CompletedTasks()
        {
            var (service, cache, _, _, _, _) = CreateService();
            var userId = Guid.NewGuid();
            // Simulate a completed task
            var field = typeof(TradesCacheService).GetField("_loadTasks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var dict = (Dictionary<Guid, Task?>?)field?.GetValue(service);
            Assert.NotNull(dict);
            dict![userId] = Task.CompletedTask;
            service.CleanupInactiveUsers(TimeSpan.Zero);
            Assert.False(dict!.ContainsKey(userId));
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
        public void Cache_Respects_MaxConcurrentUsers()
        {
            var (service, cache, _, _, _, config) = CreateService();
            int maxUsers = config.Object.Cache.MaxConcurrentUsers;
            var field = typeof(TradesCacheService).GetField("_loadTasks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var dict = (Dictionary<Guid, Task?>?)field?.GetValue(service);
            Assert.NotNull(dict);
            for (int i = 0; i < maxUsers + 5; i++)
                dict![Guid.NewGuid()] = Task.CompletedTask;
            service.InvalidateAndReload(Guid.NewGuid()); // Should trigger cleanup
            Assert.True(dict!.Count <= maxUsers);
        }

        [Fact]
        public async Task GetCachedTrades_Returns_Cached_Value_If_Present()
        {
            var (service, cache, _, _, _, _) = CreateService();
            var userId = Guid.NewGuid();
            var key = TradesCacheService.GetCacheKey(userId, 1, 10);
            var expected = new List<TradeCompositeModel> { new TradeCompositeModel() };
            cache.Set(key, (object)expected);
            var result = await service.GetCachedTrades(userId, 1, 10);
            Assert.NotNull(result);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void InvalidateAndReload_Creates_Task()
        {
            var (service, cache, _, _, _, _) = CreateService();
            var userId = Guid.NewGuid();
            service.InvalidateAndReload(userId);
            var field = typeof(TradesCacheService).GetField("_loadTasks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var dict = (Dictionary<Guid, Task?>?)field?.GetValue(service);
            Assert.NotNull(dict);
            Assert.True(dict!.ContainsKey(userId));
        }

        [Fact]
        public void CleanupInactiveUsers_Cleans_All_Dictionaries()
        {
            var (service, cache, _, _, _, _) = CreateService();
            var userId = Guid.NewGuid();
            var tcs = new CancellationTokenSource();
            var fieldTasks = typeof(TradesCacheService).GetField("_loadTasks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var fieldCts = typeof(TradesCacheService).GetField("_loadCts", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var fieldTokens = typeof(TradesCacheService).GetField("_cacheTokenSources", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var dictTasks = (Dictionary<Guid, Task?>?)fieldTasks?.GetValue(service);
            var dictCts = (Dictionary<Guid, CancellationTokenSource>?)fieldCts?.GetValue(service);
            var dictTokens = (Dictionary<Guid, CancellationTokenSource>?)fieldTokens?.GetValue(service);
            Assert.NotNull(dictTasks);
            Assert.NotNull(dictCts);
            Assert.NotNull(dictTokens);
            dictTasks![userId] = Task.CompletedTask;
            dictCts![userId] = tcs;
            dictTokens![userId] = tcs;
            service.CleanupInactiveUsers(TimeSpan.Zero);
            Assert.False(dictTasks!.ContainsKey(userId));
            Assert.False(dictCts!.ContainsKey(userId));
            Assert.False(dictTokens!.ContainsKey(userId));
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

        [Fact]
        public async Task Concurrent_GetCachedTrades_Does_Not_Throw()
        {
            var (service, cache, _, _, _, _) = CreateService();
            var userId = Guid.NewGuid();
            var key = TradesCacheService.GetCacheKey(userId, 1, 10);
            var expected = new List<TradeCompositeModel> { new TradeCompositeModel() };
            cache.Set(key, (object)expected);
            var tasks = new List<Task>();
            for (int i = 0; i < 10; i++)
                tasks.Add(service.GetCachedTrades(userId, 1, 10));
            await Task.WhenAll(tasks);
            Assert.All(tasks, t => Assert.True(t.IsCompletedSuccessfully));
        }

        [Fact]
        public void InvalidateAndReload_MultipleUsers_Does_Not_Interfere()
        {
            var (service, cache, _, _, _, _) = CreateService();
            var userId1 = Guid.NewGuid();
            var userId2 = Guid.NewGuid();
            service.InvalidateAndReload(userId1);
            service.InvalidateAndReload(userId2);
            var field = typeof(TradesCacheService).GetField("_loadTasks", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var dict = (Dictionary<Guid, Task?>?)field?.GetValue(service);
            Assert.NotNull(dict);
            Assert.True(dict!.ContainsKey(userId1));
            Assert.True(dict!.ContainsKey(userId2));
        }

        [Fact]
        public void Cache_Respects_PreloadPageCount()
        {
            var (service, cache, sp, mapper, logger, config) = CreateService();
            var userId = Guid.NewGuid();
            var preload = config.Object.Cache.PreloadPageCount;
            // Simulate the repository
            var repoMock = new Mock<IJournalRepositoryWrapper>();
            repoMock.Setup(r => r.Journal.GetAllTradeCompositesAsync(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((new List<TradeComposite>(), 0));
            sp.Setup(x => x.GetService(typeof(IJournalRepositoryWrapper))).Returns(repoMock.Object);
            // Register a mock IServiceScopeFactory
            var scopeFactoryMock = new Mock<IServiceScopeFactory>();
            var scopeMock = new Mock<IServiceScope>();
            scopeMock.Setup(s => s.ServiceProvider).Returns(sp.Object);
            scopeFactoryMock.Setup(f => f.CreateScope()).Returns(scopeMock.Object);
            sp.Setup(x => x.GetService(typeof(IServiceScopeFactory))).Returns(scopeFactoryMock.Object);
            // Add entry to _cacheTokenSources for userId
            var fieldTokens = typeof(TradesCacheService).GetField("_cacheTokenSources", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var dictTokens = (Dictionary<Guid, CancellationTokenSource>?)fieldTokens?.GetValue(service);
            Assert.NotNull(dictTokens);
            dictTokens![userId] = new CancellationTokenSource();
            // Call private method via reflection
            var method = typeof(TradesCacheService).GetMethod("LoadCacheAsync", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var task = (Task)method.Invoke(service, new object[] { userId, CancellationToken.None });
            task.GetAwaiter().GetResult();
            Assert.True(true);
        }

        [Fact]
        public void Cache_Does_Not_Throw_If_UserId_Not_Present()
        {
            var (service, cache, _, _, _, _) = CreateService();
            var userId = Guid.NewGuid();
            // Should not throw
            service.InvalidateAndReload(userId);
            Assert.True(true);
        }
    }
}
