using System.Reflection;

namespace HsR.Web.API.Configurations
{
    internal static class SwaggerExtensions
    {
        internal static IServiceCollection AddCustomSwagger(this IServiceCollection services)
        {
            return services.AddSwaggerGen(setupAction =>
            {
                var xmlCommentsFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlCommentsFullPath = Path.Combine(AppContext.BaseDirectory, xmlCommentsFile);
                setupAction.IncludeXmlComments(xmlCommentsFullPath);
            });
        }
    }
}
