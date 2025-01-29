using HsR.Journal.DataContext;

namespace HsR.Web.API.Repositories
{
    internal static class RepositoryRegistrations
    {
        internal static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserDataRepository, UserDataRepository>();
            services.AddScoped<ITradeCompositesRepository, TradeCompositesRepository>();
            services.AddScoped<ITradeElementRepository, TradeElementRepository>();
            services.AddScoped<IDataElementRepository, DataElementRepository>();
            services.AddScoped<IJournalRepositoryWrapper, JournalRepositoryWrapper>();
        }
    }
}
