using Serilog;

internal static class LoggingConfiguration
{
    internal static void ConfigureLogging()
    {
        var logPath = Path.Combine(AppContext.BaseDirectory, "hsr_journal_server.txt");
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}
