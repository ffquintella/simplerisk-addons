using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUIClient.Models;
using GUIClient.Services;
using Microsoft.Extensions.Localization;
using Splat;

namespace GUIClient.Views;

public partial class Login : Window
{
    private IRegistrationService _registrationService;
    private ILocalizationService _localizationService;
    private IEnvironmentService _environmentService;
    private IStringLocalizer _localizer;

    public Login()
    {
        _registrationService = GetService<IRegistrationService>();
        _localizationService = GetService<ILocalizationService>();
        _environmentService = GetService<IEnvironmentService>();
        _localizer = _localizationService.GetLocalizer();
        
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
        if (!_registrationService.IsRegistered)
        {
            var result = _registrationService.Register(_environmentService.DeviceID);

            if (result.Result == RequestResult.Success)
            {
                var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                    .GetMessageBoxStandardWindow(_localizer["Warning"], _localizer["NoRegistrationMSG"] 
                                                                        + " " +  result.RequestID );
                messageBoxStandardWindow.Show();
            }
            else
            {
                
            }
            


        }
    }

    private class RegistrationResult
    {
    }

    private static T GetService<T>() => Locator.Current.GetService<T>();
}