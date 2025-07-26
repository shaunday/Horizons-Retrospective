using System.Reflection;
using Microsoft.OpenApi.Models;

namespace AssetsFlowWeb.API.Configurations
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
