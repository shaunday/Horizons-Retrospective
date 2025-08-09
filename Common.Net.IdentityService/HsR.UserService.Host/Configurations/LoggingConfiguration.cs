using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace HsR.UserService.Host.Configurations
{
    internal static class LoggingConfiguration
    {
        internal static WebApplicationBuilder ConfigureLogging(this WebApplicationBuilder builder)
        {
            var logPath = Path.Combine(AppContext.BaseDirectory, "hsr_userservice.txt");
            bool isDev = builder.Environment.IsDevelopment();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(isDev ? LogEventLevel.Debug : LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Hook into Generic Host (used by Aspire)
            builder.Host.UseSerilog();

            return builder;
        }
    }
}
