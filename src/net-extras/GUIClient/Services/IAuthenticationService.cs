using Avalonia.Controls;

namespace GUIClient.Services;

public interface IAuthenticationService
{
    bool IsAuthenticated { get; set; }

    void TryAuthenticate(Window parentWindow);

}