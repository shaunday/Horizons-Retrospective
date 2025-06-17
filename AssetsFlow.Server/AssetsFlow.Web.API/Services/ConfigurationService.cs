using Microsoft.Extensions.Configuration;
using HsR.Web.API.Configuration;

namespace HsR.Web.API.Services
{
    public interface IConfigurationService
    {
        PaginationSettings Pagination { get; }
        CacheSettings Cache { get; }
    }

    public class ConfigurationService : IConfigurationService
    {
        private readonly PaginationSettings _pagination;
        private readonly CacheSettings _cache;

        public ConfigurationService(IConfiguration configuration)
        {
            _pagination = configuration.GetSection("PaginationSettings").Get<PaginationSettings>() ?? new PaginationSettings();
            _cache = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? new CacheSettings();
        }

        public PaginationSettings Pagination => _pagination;
        public CacheSettings Cache => _cache;
    }
} 