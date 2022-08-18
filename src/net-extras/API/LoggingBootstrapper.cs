using Serilog;
using Serilog.Events;

namespace API;

public static class LoggingBootstrapper
{
    public static void RegisterLogging()
    {
        var logDir = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + "/SRAPIServer";
        Directory.CreateDirectory(logDir);
        var logPath = logDir + "/logs";

        Log.Logger = new LoggerConfiguration()
            .WriteTo.RollingFile(logPath, outputTemplate: "{Timestamp:dd/MM/yy HH:mm:ss} [{Level:u4}] {Message:lj}{NewLine}{Exception}", 
                restrictedToMinimumLevel: LogEventLevel.Debug)
            .MinimumLevel.Verbose()
            .CreateLogger();
    }
}