using AssetsFlowWeb.Services.Models.Journal;
using AutoMapper;
using HsR.Common.AspNet.Authentication;
using HsR.Common.Services.Caching;
using HsR.Journal.DataContext;
using HsR.Web.API.Services;
using HsR.Web.API.Settings;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
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

        private (TradesCacheService service, IMemoryCache cache, Mock<IJournalRepositoryWrapper> journalMock) CreateService(int? sizeLimit = null)
        {
            var cache = CreateMemoryCache(sizeLimit);
            var spMock = new Mock<IServiceProvider>();
            var journalMock = new Mock<IJournalRepositoryWrapper>();
            var journalRepoMock = new Mock<IJournalRepository>();
            journalMock.Setup(j => j.Journal).Returns(journalRepoMock.Object);

            var serviceProviderMock = new Mock<IServiceScope>();
            serviceProviderMock.Setup(s => s.ServiceProvider).Returns(spMock.Object);

            var scopeFactoryMock = new Mock<IServiceScopeFactory>();
            scopeFactoryMock.Setup(f => f.CreateScope()).Returns(serviceProviderMock.Object);
            spMock.Setup(sp => sp.GetService(typeof(IServiceScopeFactory))).Returns(scopeFactoryMock.Object);
            spMock.Setup(sp => sp.GetService(typeof(IJournalRepositoryWrapper))).Returns(journalMock.Object);

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
                },
                Pagination = new PaginationSettings { DefaultPageSize = 10 }
            };

            var service = new TradesCacheService(cache, spMock.Object, mapper, logger, configStub);
            return (service, cache, journalMock);
        }

        [Fact]
        public async Task GetCachedTrades_Returns_Null_If_Not_Present()
        {
            var (service, _, _) = CreateService();
            var userId = Guid.NewGuid();
            var result = await service.GetCachedTrades(userId, 1, 10);
            Assert.Null(result);
        }

        [Fact]
        public void InvalidateAndReload_PublicMethod_Runs()
        {
            var (service, _, _) = CreateService();
            var userId = Guid.NewGuid();

            service.InvalidateAndReload(userId);
        }

        [Fact]
        public void CleanupInactive_PublicMethod_Runs()
        {
            var (service, _, _) = CreateService();
            var userId = Guid.NewGuid();

            service.CleanupInactive(TimeSpan.Zero);
        }

        [Fact]
        public void GetCachedTotalCount_DefaultsToZero_WhenNothingCached()
        {
            var (service, _, _) = CreateService();
            var userId = Guid.NewGuid();

            var totalCount = service.GetCachedTotalCount(userId);

            Assert.Equal(0, totalCount);
        }

        [Fact]
        public async Task SetAndGetCachedTrades_WorksWithPublicMethods()
        {
            var (service, cache, _) = CreateService();
            var userId = Guid.NewGuid();
            var trades = new List<TradeCompositeModel>
            {
                new TradeCompositeModel { Id = 1 }
            };

            // Set using public Set method
            service.Set(userId, trades, "1_10");

            var result = await service.GetCachedTrades(userId, 1, 10);
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result?.FirstOrDefault()?.Id);
        }
    }
}
