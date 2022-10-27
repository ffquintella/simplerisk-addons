using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using Serilog.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace API;

public static class LoggingBootstrapper
{
    public static void RegisterLogging(IServiceCollection services,IConfiguration config)
    {
        var logDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/SRAPIServer";
        Directory.CreateDirectory(logDir);
        var logPath = logDir + "/logs";

        LoggingLevelSwitch defaultLoggingLevel = new LoggingLevelSwitch();
        switch (config["Logging:LogLevel:Default"])
        {
            case "Information":
                defaultLoggingLevel.MinimumLevel = LogEventLevel.Information;
                break;
            case "Warning":
                defaultLoggingLevel.MinimumLevel = LogEventLevel.Warning;
                break;
            case "Error":
                defaultLoggingLevel.MinimumLevel = LogEventLevel.Error;
                break;
            case "Debug":
                defaultLoggingLevel.MinimumLevel = LogEventLevel.Debug;
                break;
            case "Fatal":
                defaultLoggingLevel.MinimumLevel = LogEventLevel.Fatal;
                break;
            case "Verbose":
                defaultLoggingLevel.MinimumLevel = LogEventLevel.Verbose;
                break;
            default:
                defaultLoggingLevel.MinimumLevel = LogEventLevel.Warning;
                break;
        }
        
        LoggingLevelSwitch microsoftLoggingLevel = new LoggingLevelSwitch();
        switch (config["Logging:LogLevel:Microsoft"])
        {
            case "Information":
                microsoftLoggingLevel.MinimumLevel = LogEventLevel.Information;
                break;
            case "Warning":
                microsoftLoggingLevel.MinimumLevel = LogEventLevel.Warning;
                break;
            case "Error":
                microsoftLoggingLevel.MinimumLevel = LogEventLevel.Error;
                break;
            case "Debug":
                microsoftLoggingLevel.MinimumLevel = LogEventLevel.Debug;
                break;
            case "Fatal":
                microsoftLoggingLevel.MinimumLevel = LogEventLevel.Fatal;
                break;
            case "Verbose":
                microsoftLoggingLevel.MinimumLevel = LogEventLevel.Verbose;
                break;
            default:
                microsoftLoggingLevel.MinimumLevel = LogEventLevel.Warning;
                break;
        }
        
        var logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Default", defaultLoggingLevel)
            .MinimumLevel.Override("Microsoft", microsoftLoggingLevel)
            .WriteTo.Console()
            .WriteTo.RollingFile(logPath, fileSizeLimitBytes: 10000)
            .CreateLogger();
        var factory = new SerilogLoggerFactory(logger);

        Log.Logger = logger;
        
        services.AddSingleton<ILoggerFactory>(factory);
        
        services.AddSingleton<ILogger>(logger);
        

    }
}