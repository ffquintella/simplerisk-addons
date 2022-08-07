namespace GUIClient.Services;

public interface IEnvironmentService
{
    string NewLine { get; }

    bool Is64BitProcess { get; }
    
    string ApplicationData { get; }
    
    string ApplicationDataFolder { get; }

    string? GetEnvironmentVariable(string variableName);
}