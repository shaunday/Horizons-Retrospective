using AssetsFlowWeb.Services.Models;
using AutoMapper;
using HsR.Common.Services.Caching;
using HsR.Journal.DataContext;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HsR.Web.API.Services
{
    public class UserDataCacheService : CacheServiceBase<Guid, UserDataDTO>
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IMapper _mapper;
        private readonly IConfigurationService _config;

        protected override string CacheKeyPrefix => "user_data";

        public UserDataCacheService(
            IMemoryCache memoryCache,
            ILogger<UserDataCacheService> logger,
            IServiceProvider serviceProvider,
            IMapper mapper,
            IConfigurationService config)
            : base(
                memoryCache,
                logger,
                TimeSpan.FromMinutes(config.Cache.CacheDurationMinutes),
                config.Cache.MaxConcurrentUsers)
        {
            _serviceProvider = serviceProvider;
            _mapper = mapper;
            _config = config;
        }

        protected override async Task<UserDataDTO?> LoadFromSourceAsync(Guid userId, CancellationToken token)
        {
            using var scope = _serviceProvider.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IUserDataRepository>();

            var userData = await repo.GetOrCreateUserDataAsync(userId); //todo pass cancelation token?

            if (userData == null)
                return null;

            return _mapper.Map<UserDataDTO>(userData);
        }
    }
}