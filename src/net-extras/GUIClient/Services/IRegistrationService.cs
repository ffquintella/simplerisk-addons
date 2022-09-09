using GUIClient.Models;

namespace GUIClient.Services;

public interface IRegistrationService
{
    bool IsRegistered { get; }
    bool IsAccepted { get; }
    RegistrationSolicitationResult Register(string ID, bool force = false);

    bool CheckAcceptance(string Id, bool force = false);

}