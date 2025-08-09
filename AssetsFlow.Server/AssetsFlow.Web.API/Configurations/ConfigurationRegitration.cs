using HsR.Web.API.Settings;
using HsR.Web.API.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using HsR.Common.AspNet.Authentication;

namespace AssetsFlowWeb.API.Configurations
{
    public static class ConfigurationConfig
    {
        public static IServiceCollection AddConfigurationServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PaginationSettings>(configuration.GetSection("PaginationSettings"));
            services.Configure<CacheSettings>(configuration.GetSection("CacheSettings"));
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            services.AddSingleton<IConfigurationService, ConfigurationService>();

            return services;
        }
    }
} 