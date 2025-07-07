using HsR.Journal.Seeder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;

namespace AssetsFlowWeb.API.Configurations
{
    internal static class DevConfig
    {
        internal static IServiceCollection ApplyDevConfig(this IServiceCollection services)
        {
            IdentityModelEventSource.ShowPII = true;
            services.AddScoped<DatabaseSeeder>();

            return services;
        }
    }
} 