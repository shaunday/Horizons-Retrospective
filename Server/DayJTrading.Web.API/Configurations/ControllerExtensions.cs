using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;

namespace DayJTrading.Web.API.Configurations
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
            return services.AddControllers(options =>
            {
                options.ReturnHttpNotAcceptable = true;
                options.OutputFormatters.Insert(0, new SystemTextJsonOutputFormatter(new JsonSerializerOptions
                {
                    WriteIndented = true
                }));
            });
        }
    }

}
