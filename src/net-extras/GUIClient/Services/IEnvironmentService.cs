namespace GUIClient.Services;

public interface IEnvironmentService
{
    string NewLine { get; }

    bool Is64BitProcess { get; }

    string? GetEnvironmentVariable(string variableName);
}