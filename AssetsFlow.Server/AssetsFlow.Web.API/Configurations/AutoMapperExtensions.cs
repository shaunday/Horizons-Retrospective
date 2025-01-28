using AutoMapper;
using AutoMapper.EquivalencyExpression;
using HsR.Web.API.Mapping;

namespace HsR.Web.API.Configurations
{
    internal static class AutoMapperExtensions
    {
        internal static IServiceCollection AddCustomAutoMapper(this IServiceCollection services)
        {
            Action<IMapperConfigurationExpression> configAction = (mce) =>
            {
                mce.AddMaps(typeof(EntitiesToDtosMappingProfile));
                mce.AddCollectionMappers();
            };
            return services.AddAutoMapper(configAction);
        }
    }
}
