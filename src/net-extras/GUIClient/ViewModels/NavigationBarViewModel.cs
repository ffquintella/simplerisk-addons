using System;
using System.Threading.Tasks;
using Avalonia.Controls;
using GUIClient.Configuration;
using GUIClient.Services;
using GUIClient.Views;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class NavigationBarViewModel: ViewModelBase
{

    private ILocalizationService _localization;
    private ServerConfiguration _configuration;
    private IAuthenticationService _authenticationService;
    private bool _isEnabled = false;
    public string? _loggedUser;

    public Boolean IsEnabled
    {
        get => _isEnabled;
        set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
    }

    public String LoggedUser
    {
        get => _loggedUser;
        set => this.RaiseAndSetIfChanged(ref _loggedUser, value);
    }

    public NavigationBarViewModel(
        ILocalizationService localization,
        ServerConfiguration configuration,
        IAuthenticationService authenticationService)
    {
        _localization = localization;
        _configuration = configuration;
        _authenticationService = authenticationService;
        
        Task.Run(() => UpdateAuthenticationStatus());
        
        
    }

    public async Task UpdateAuthenticationStatus()
    {
        while (!_authenticationService.IsAuthenticated)
        {
            Task.Delay(1000);
        }

        IsEnabled = true;
        if (_authenticationService!.AuthenticatedUserInfo == null) _authenticationService.GetAuthenticatedUserInfo();
        LoggedUser = _authenticationService!.AuthenticatedUserInfo!.UserName!;
    }
    
    public void OnSettingsCommand(NavigationBar? parentControl)
    {

        var dialog = new Settings()
        {
            DataContext = new SettingsViewModel(_localization, _configuration)
        };
        dialog.ShowDialog( parentControl.ParentWindow );

    }
    
    public void OnAccountCommand(NavigationBar? parentControl)
    {
        if (_authenticationService == null)
        {
            _authenticationService!.GetAuthenticatedUserInfo();
        }

        var dialog = new UserInfo()
        {
            DataContext = new UserInfoViewModel(_authenticationService.AuthenticatedUserInfo!)
        };
        dialog.ShowDialog( parentControl.ParentWindow );

    }
}