using HsR.Journal.DataContext;

namespace HsR.Web.API.Repositories
{
    public static class RepositoryRegistrations
    {
        public static void AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IGeneralDataRepository, GeneralDataRepository>();
            services.AddScoped<ITradeCompositeRepository, TradeCompositeRepository>();
            services.AddScoped<ITradeElementRepository, TradeElementRepository>();
            services.AddScoped<IDataElementRepository, DataElementRepository>();
            services.AddScoped<IJournalRepository, JournalRepository>();
        }
    }
}
