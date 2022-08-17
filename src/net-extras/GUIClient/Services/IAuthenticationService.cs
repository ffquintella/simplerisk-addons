using Avalonia.Controls;
using GUIClient.Models;

namespace GUIClient.Services;

public interface IAuthenticationService
{
    bool IsAuthenticated { get; set; }
    
    AuthenticationCredential AuthenticationCredential { get; set; }

    void TryAuthenticate(Window parentWindow);

}