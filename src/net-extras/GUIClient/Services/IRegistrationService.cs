namespace GUIClient.Services;

public interface IRegistrationService
{
    bool IsRegistered { get; }
    void Register(string ID);
    
}