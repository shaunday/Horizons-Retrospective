using HsR.Journal.DataContext;
using HsR.Journal.Repository.Services.CompositeRepo;

namespace HsR.Web.API.Repositories
{
    internal static class RepositoryRegistrations
    {
        internal static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserDataRepository, UserDataRepository>();
            services.AddScoped<IJournalRepository, JournalRepository>();
            services.AddScoped<ITradeCompositeRepository, TradeCompositeRepository>();
            services.AddScoped<ITradeElementRepository, TradeElementRepository>();
            services.AddScoped<IDataElementRepository, DataElementRepository>();
            services.AddScoped<IJournalRepositoryWrapper, JournalRepositoryWrapper>();
        }
    }
}
