using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using GUIClient.Models;
using GUIClient.Services;
using GUIClient.ViewModels;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Models;
using Microsoft.Extensions.Localization;
using Model.Authentication;
using Splat;

namespace GUIClient.Views;

public partial class Login : Window
{
    private IRegistrationService _registrationService;
    private ILocalizationService _localizationService;
    private IEnvironmentService _environmentService;
    private IStringLocalizer _localizer;
    private IMutableConfigurationService _mutableConfigurationService;
    private IAuthenticationService _authenticationService;

    
    public Login()
    {
        DataContext = new LoginViewModel();
  
        _registrationService = GetService<IRegistrationService>();
        _localizationService = GetService<ILocalizationService>();
        _environmentService = GetService<IEnvironmentService>();
        _mutableConfigurationService = GetService<IMutableConfigurationService>();
        _localizer = _localizationService.GetLocalizer();
        _authenticationService = GetService<IAuthenticationService>();
        
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
        
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }

    private void OnOpened(object? sender, EventArgs eventArgs)
    {
        //Checking if registration was donne
        if (!_registrationService.IsRegistered)
        {
            var result = _registrationService.Register(_environmentService.DeviceID);

            if (result.Result == RequestResult.Success)
            {
                var assets = AvaloniaLocator.Current.GetService<IAssetLoader>();
                var bitmap = new Bitmap(assets!.Open(new Uri("avares://GUIClient/Assets/Hex-Warning.ico")));
                
                var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
                    {
                        ContentTitle = _localizer["Warning"],
                        //ContentHeader = "header",
                        ContentMessage = _localizer["NoRegistrationMSG"]  + " " +  result.RequestID ,
                        Icon = MessageBox.Avalonia.Enums.Icon.Warning,
                        //WindowIcon = new WindowIcon(bitmap)
                    });
                        
                messageBoxStandardWindow.Show();
            }
            else
            {
                var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(_localizer["Warning"], _localizer["RegistrationErrorMSG"]);
                messageBoxStandardWindow.Show();
            }
        }
        else
        {
            
            // checking if registration was accepted
            if (!_registrationService.IsAccepted)
            {
                if (!_registrationService.CheckAcceptance(_environmentService.DeviceID))
                {
                    var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                        .GetMessageBoxStandardWindow(_localizer["Warning"], _localizer["RegistrationNotAcceptedMSG"] 
                                                                            + " " + _mutableConfigurationService.GetConfigurationValue("RegistrationID"));
                    messageBoxStandardWindow.Show();
                }
            }
            else
            {
                ((LoginViewModel) DataContext!).IsAccepted = true;
                
            }
        }
    }

    private class RegistrationResult
    {
    }

    private static T GetService<T>() => Locator.Current.GetService<T>();
}