using System;
using Avalonia.Controls;
using Avalonia.Interactivity;
using GUIClient.Configuration;
using GUIClient.Services;
using GUIClient.ViewModels;
using Splat;

namespace GUIClient.Views
{
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
             
        }
        
        
        
        private void LoadCheck(object? sender, EventArgs eventArgs)
        {
            var authenticationService = GetService<IAuthenticationService>();
            if(authenticationService.IsAuthenticated == false) authenticationService.TryAuthenticate(this);
        } 
        public void btn_SettingsOnClick( object? sender, RoutedEventArgs args )
        {
            var localizationService = GetService<ILocalizationService>();
            var serverConfiguration = GetService<ServerConfiguration>();
            
            var dialog = new Settings()
            {
                DataContext = new SettingsViewModel(localizationService, serverConfiguration)
            };
            dialog.ShowDialog( this );

        }
        private static T GetService<T>() => Locator.Current.GetService<T>();
    }
}