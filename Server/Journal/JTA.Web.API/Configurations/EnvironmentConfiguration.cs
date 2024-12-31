using Microsoft.IdentityModel.Logging;

namespace JTA.Web.API.Configurations
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
