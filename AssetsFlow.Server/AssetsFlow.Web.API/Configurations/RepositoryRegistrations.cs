using HsR.Journal.DataContext;

namespace HsR.Web.API.Repositories
{
    internal static class RepositoryRegistrations
    {
        internal static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IGeneralDataRepository, GeneralDataRepository>();
            services.AddScoped<ITradeCompositeRepository, TradeCompositeRepository>();
            services.AddScoped<ITradeElementRepository, TradeElementRepository>();
            services.AddScoped<IDataElementRepository, DataElementRepository>();
            services.AddScoped<IJournalRepositoryWrapper, JournalRepositoryWrapper>();
        }
    }
}
