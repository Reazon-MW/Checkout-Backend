using Microsoft.Extensions.Logging;

internal static class Log
{
    public static ILogger Create<T>()
    {
        var loggerFactory = LoggerFactory.Create(builder =>
        {
            builder
                .AddFilter("Microsoft", LogLevel.Warning)
                .AddFilter("System", LogLevel.Warning)
                .AddFilter("LoggingConsoleApp.Program", LogLevel.Debug)
                .AddConsole()
                .AddEventLog();
        });
        return loggerFactory.CreateLogger<T>();
    }
}