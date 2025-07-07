using Serilog;
using Serilog.Events;
using Microsoft.AspNetCore.Builder;

namespace AssetsFlowWeb.API.Configurations
{
    public static class LoggingConfig
    {
        public static WebApplicationBuilder ConfigureAssetsFlowLogging(this WebApplicationBuilder builder)
        {
            var logPath = Path.Combine(AppContext.BaseDirectory, "hsr_journal_server.txt");

            bool isDev = builder.Environment.IsDevelopment();
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Is(isDev ? LogEventLevel.Debug : LogEventLevel.Warning)
                .WriteTo.Console()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .CreateLogger();
            builder.Host.UseSerilog();
            return builder;
        }
    }
}
