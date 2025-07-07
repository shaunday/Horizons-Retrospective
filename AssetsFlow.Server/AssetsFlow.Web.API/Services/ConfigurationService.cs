using HsR.Web.API.Settings;

namespace HsR.Web.API.Services
{
    public interface IConfigurationService
    {
        PaginationSettings Pagination { get; }
        CacheSettings Cache { get; }
        JwtSettings Jwt { get; }
    }

    public class ConfigurationService : IConfigurationService
    {
        private readonly PaginationSettings _pagination;
        private readonly CacheSettings _cache;
        private readonly JwtSettings _jwt;

        public ConfigurationService(IConfiguration configuration)
        {
            _pagination = configuration.GetSection("PaginationSettings").Get<PaginationSettings>() ?? new PaginationSettings();
            _cache = configuration.GetSection("CacheSettings").Get<CacheSettings>() ?? new CacheSettings();
            _jwt = configuration.GetSection(JwtSettings.SectionName).Get<JwtSettings>() ?? new JwtSettings();
        }

        public PaginationSettings Pagination => _pagination;
        public CacheSettings Cache => _cache;
        public JwtSettings Jwt => _jwt;
    }
} 