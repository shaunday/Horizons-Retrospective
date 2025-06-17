using Asp.Versioning;
using AutoMapper;
using HsR.Web.API.Mapping;

namespace HsR.Web.API.Configurations
{
    internal static class ApiVersioning
    {
        internal static IServiceCollection AddCustomApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.ReportApiVersions = true;
            });

            return services;
        }
    }
}
