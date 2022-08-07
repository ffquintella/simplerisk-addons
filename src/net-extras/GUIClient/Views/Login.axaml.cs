using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using GUIClient.Services;
using Splat;

namespace GUIClient.Views;

public partial class Login : Window
{
    private IRegistrationService _registrationService;
    public Login()
    {
        _registrationService = GetService<IRegistrationService>();
        
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
                .GetMessageBoxStandardWindow("title", "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed...");
            messageBoxStandardWindow.Show();
        }
    }
    private static T GetService<T>() => Locator.Current.GetService<T>();
}