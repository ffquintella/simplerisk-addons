using GUIClient.Models;

namespace GUIClient.Services;

public interface IRegistrationService
{
    bool IsRegistered { get; }
    RegistrationSolicitationResult Register(string ID);
    
}