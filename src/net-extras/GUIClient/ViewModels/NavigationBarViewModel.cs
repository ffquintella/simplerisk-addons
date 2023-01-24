using System;
using System.Reactive;
using System.Threading.Tasks;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.LogicalTree;
using GUIClient.Configuration;
using GUIClient.Models;
using GUIClient.Services;
using GUIClient.Views;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class NavigationBarViewModel: ViewModelBase
{

    
    private ServerConfiguration _configuration;
    private bool _isEnabled = false;
    private bool _isAdmin = false;
    private bool _hasAssessmentPermission = false;
    public string? _loggedUser;

    public Boolean IsAdmin
    {
        get
        {
            if (_isEnabled) return _isAdmin;
            return false;
        }
        set => this.RaiseAndSetIfChanged(ref _isAdmin, value);
    }
    public Boolean IsEnabled
    {
        get => _isEnabled;
        set => this.RaiseAndSetIfChanged(ref _isEnabled, value);
    }

    public Boolean HasAssessmentPermission
    {
        get
        {
            if (!_isEnabled) return false;
            return _hasAssessmentPermission;
        }
        set => this.RaiseAndSetIfChanged(ref _hasAssessmentPermission, value);
    }

    
    public String? LoggedUser
    {
        get => _loggedUser;
        set => this.RaiseAndSetIfChanged(ref _loggedUser, value);
    }

    public ReactiveCommand<Button, Unit> BtDashboardClicked { get; }
    public ReactiveCommand<Window, Unit> BtSettingsClicked { get; }
    public ReactiveCommand<MainWindow, Unit> BtDeviceClicked { get; }
    
    public NavigationBarViewModel(
        ServerConfiguration configuration)
    {
        
        _configuration = configuration;
        
        //Task.Run(() => UpdateAuthenticationStatus());

        AuthenticationService.AuthenticationSucceeded += (obj, args) =>
        {
            Initialize();
        };
        
        BtDashboardClicked = ReactiveCommand.Create<Button>(ExecuteOpenDashboard);
        BtSettingsClicked = ReactiveCommand.Create<Window>(ExecuteOpenSettings);
        BtDeviceClicked = ReactiveCommand.Create<MainWindow>(ExecuteOpenDevice);
    }

    public void Initialize()
    {
        UpdateAuthenticationStatus();
    }
    
    public void UpdateAuthenticationStatus()
    {
        /*while (!_authenticationService.IsAuthenticated)
        {
            Task.Delay(1000);
        }*/

        IsEnabled = true;
        if (AuthenticationService!.AuthenticatedUserInfo == null) AuthenticationService.GetAuthenticatedUserInfo();
        LoggedUser = AuthenticationService!.AuthenticatedUserInfo!.UserName!;
        if (AuthenticationService.AuthenticatedUserInfo.UserRole == "Administrator") IsAdmin = true;
        if (AuthenticationService.AuthenticatedUserInfo.UserPermissions!.Contains("assessments")) HasAssessmentPermission = true;
    }

    public void ExecuteOpenSettings(Window sender)
    {
        var dialog = new Settings()
        {
            DataContext = new SettingsViewModel(_configuration),
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        dialog.ShowDialog( sender );

    }

    public void OnSettingsCommand(NavigationBar parentControl)
    {
        
        var dialog = new Settings()
        {
            DataContext = new SettingsViewModel(_configuration),
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        dialog.ShowDialog( parentControl.ParentWindow );

    }
    
    public void OnAccountCommand(NavigationBar parentControl)
    {
        if (AuthenticationService == null)
        {
            AuthenticationService!.GetAuthenticatedUserInfo();
        }

        var dialog = new UserInfo()
        {
            DataContext = new UserInfoViewModel(AuthenticationService.AuthenticatedUserInfo!),
            WindowStartupLocation = WindowStartupLocation.CenterOwner
        };
        dialog.ShowDialog( parentControl.ParentWindow );

    }
    
    public void ExecuteOpenDevice(MainWindow window)
    {
        ((MainWindowViewModel)window.DataContext!)
            .NavigateTo(AvaliableViews.Devices);
        
    }
    
    public void OnDeviceCommand(NavigationBar parentControl)
    {
        ((MainWindowViewModel)parentControl.ParentWindow.DataContext!)
            .NavigateTo(AvaliableViews.Devices);
        
    }
    
    public void  ExecuteOpenDashboard(Button sender)
    {
        var button = ( sender as Button )!;
        
        ((MainWindowViewModel)(button.Parent).Parent.DataContext!)
            .NavigateTo(AvaliableViews.Dashboard);
    }
    
    public void OnDashboardCommand(NavigationBar parentControl)
    {
        ((MainWindowViewModel)parentControl.ParentWindow.DataContext!)
            .NavigateTo(AvaliableViews.Dashboard);
        
    }
    public void OnAssessmentCommand(NavigationBar parentControl)
    {
        ((MainWindowViewModel)parentControl.ParentWindow.DataContext!)
            .NavigateTo(AvaliableViews.Assessment);
        
    }
}