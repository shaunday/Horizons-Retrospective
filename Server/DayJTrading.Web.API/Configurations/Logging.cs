using Serilog;

internal static class LoggingConfiguration
{
    internal static void ConfigureLogging()
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.File("/logs/traJedi.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
    }
}
