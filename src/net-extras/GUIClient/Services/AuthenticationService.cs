using Avalonia.Controls;
using GUIClient.ViewModels;
using GUIClient.Views;
using Microsoft.Extensions.Logging;

namespace GUIClient.Services;

public class AuthenticationService: IAuthenticationService
{
    public bool IsAuthenticated { get; set; } = false;
    public void TryAuthenticate(Window parentWindow)
    {
        var dialog = new Login()
        {
            DataContext = new LoginViewModel()
        };
        dialog.ShowDialog( parentWindow );
    }

    private ILogger<AuthenticationService> _logger;
    public AuthenticationService(ILoggerFactory loggerFactory)
    {
        _logger = loggerFactory.CreateLogger<AuthenticationService>();
    }
}