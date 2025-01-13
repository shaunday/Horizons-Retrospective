using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace HsR.Web.API.Configurations
{
    /// <summary>
    /// Extension methods for configuring controllers.
    /// </summary>
    internal static class ControllerExtensions
    {
        /// <summary>
        /// Adds configured controllers to the service collection.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <returns>The updated service collection.</returns>
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
