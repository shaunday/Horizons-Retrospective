using HsR.Web.API.Controllers;
using Microsoft.AspNetCore.Mvc.Formatters;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace HsR.Web.API.Configurations
{
    internal static class ControllersConfiguration
    {
        internal static IMvcBuilder AddConfiguredControllers(this IServiceCollection services)
        {
            // Register services in the DI container
            services.AddTransient<CustomExceptionFilterAttribute>();


            return services
                .AddControllers(options =>
                {
                    // Ensure that the server returns HTTP 406 if the requested content type is not acceptable
                    options.ReturnHttpNotAcceptable = true;

                    // Register the custom exception filter
                    options.Filters.Add<CustomExceptionFilterAttribute>();
                })
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.WriteIndented = true;
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                    options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                });
        }
    }
}
