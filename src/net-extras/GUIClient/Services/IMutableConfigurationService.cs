namespace GUIClient.Services;

public interface IMutableConfigurationService
{
    bool IsInitialized { get; }

    void Initialize();
}