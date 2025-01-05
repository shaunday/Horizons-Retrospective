using Microsoft.IdentityModel.Logging;

namespace HsR.Web.API.Configurations
{
    public static class EnvironmentConfiguration
    {
        public static void ConfigureForEnvironment(this WebApplicationBuilder builder)
        {
            if (builder.Environment.IsDevelopment())
            {
                IdentityModelEventSource.ShowPII = true;
            }
        }
    }

}
