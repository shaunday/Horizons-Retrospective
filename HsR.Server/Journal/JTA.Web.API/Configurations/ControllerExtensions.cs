using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace HsR.Web.API.Configurations
{
    internal static class ControllerExtensions
    {
        internal static IMvcBuilder AddConfiguredControllers(this IServiceCollection services)
        {
            return services
                .AddControllers(options =>
                {
                    // Ensure that the server returns HTTP 406 if the requested content type is not acceptable
                    options.ReturnHttpNotAcceptable = true;
                })
                .AddJsonOptions(options =>
                {
                    // Configure the JSON serializer to write indented JSON
                    options.JsonSerializerOptions.WriteIndented = true;
                });
        }
    }
}
