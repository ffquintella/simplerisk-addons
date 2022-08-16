using Avalonia.Controls;
using GUIClient.ViewModels;
using GUIClient.Views;
using Microsoft.Extensions.Logging;

namespace GUIClient.Services;

public class AuthenticationService: IAuthenticationService
{
    public bool IsAuthenticated { get; set; } = false;

    private ILogger<AuthenticationService> _logger;
    private IRegistrationService _registrationService;
    public AuthenticationService(ILoggerFactory loggerFactory, IRegistrationService registrationService)
    {
        _logger = loggerFactory.CreateLogger<AuthenticationService>();
        _registrationService = registrationService;
    }
    
    public void TryAuthenticate(Window parentWindow)
    {
        var dialog = new Login();
        dialog.ShowDialog( parentWindow );
    }
}