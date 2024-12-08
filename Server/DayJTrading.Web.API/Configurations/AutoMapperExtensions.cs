using AutoMapper;
using AutoMapper.EquivalencyExpression;
using DayJT.Web.API.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace DayJTrading.Web.API.Configurations
{
    internal static class AutoMapperExtensions
    {
        internal static IServiceCollection AddCustomAutoMapper(this IServiceCollection services)
        {
            Action<IMapperConfigurationExpression> configAction = (mce) =>
            {
                mce.AddMaps(typeof(JournalObjectsMappingProfile));
                mce.AddCollectionMappers();
            };
            return services.AddAutoMapper(configAction);
        }
    }
}
