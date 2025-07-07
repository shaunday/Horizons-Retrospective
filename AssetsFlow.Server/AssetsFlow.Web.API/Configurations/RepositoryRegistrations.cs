using HsR.Journal.DataContext;
using HsR.Journal.Repository.Services.CompositeRepo;
using Microsoft.Extensions.DependencyInjection;

namespace AssetsFlowWeb.API.Configurations
{
    internal static class RepositoryRegistrations
    {
        internal static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserDataRepository, UserDataRepository>();
            services.AddScoped<IJournalRepository, JournalRepository>();
            services.AddScoped<ITradeCompositeRepository, TradeCompositeRepository>();
            services.AddScoped<ITradeElementRepository, TradeElementRepository>();
            services.AddScoped<IDataElementRepository, DataElementRepository>();
            services.AddScoped<IJournalRepositoryWrapper, JournalRepositoryWrapper>();
            return services;
        }
    }
}
