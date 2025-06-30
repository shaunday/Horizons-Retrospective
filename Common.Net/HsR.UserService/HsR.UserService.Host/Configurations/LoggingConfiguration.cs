using Serilog;
using Serilog.Events;
using System;
using System.IO;

namespace HsR.UserService.Host.Configurations
{
    internal static class LoggingConfiguration
    {
        internal static void ConfigureLogging(bool isDev)
        {
            var logPath = Path.Combine(AppContext.BaseDirectory, "hsr_userservice.txt");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(isDev ? LogEventLevel.Debug : LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
        }
    }
}
