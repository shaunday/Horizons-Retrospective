using Serilog;
using Serilog.Events;

internal static class LoggingConfiguration
{
    internal static void ConfigureLogging(bool isDev)
    {
        var logPath = Path.Combine(AppContext.BaseDirectory, "hsr_journal_server.txt");

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Is(isDev ? LogEventLevel.Debug : LogEventLevel.Warning)
            .WriteTo.Console()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();

    }
}
