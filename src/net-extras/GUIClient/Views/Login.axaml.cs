using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUIClient.Services;
using Microsoft.Extensions.Localization;
using Splat;

namespace GUIClient.Views;

public partial class Login : Window
{
    private IRegistrationService _registrationService;
    private ILocalizationService _localizationService;
    private IStringLocalizer _localizer;

    public Login()
    {
        _registrationService = GetService<IRegistrationService>();
        _localizationService = GetService<ILocalizationService>();
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
            var messageBoxStandardWindow = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(_localizer["Warning"], _localizer["NoRegistrationMSG"]);
            messageBoxStandardWindow.Show();
        }
    }
    private static T GetService<T>() => Locator.Current.GetService<T>();
}