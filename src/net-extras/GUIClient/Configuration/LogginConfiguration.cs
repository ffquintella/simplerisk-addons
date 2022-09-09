using Serilog.Events;

namespace GUIClient.Configuration;

public class LoggingConfiguration
{
    public string LogFileName { get; set; } = "log.txt";

    public long LimitBytes { get; set; }

    public LogEventLevel DefaultLogLevel { get; set; }

    public LogEventLevel MicrosoftLogLevel { get; set; }
}