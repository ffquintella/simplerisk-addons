using Avalonia.Controls;
using Avalonia.Interactivity;
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
        public void btn_SettingsOnClick( object? sender, RoutedEventArgs args )
        {
            var localizationService = GetService<ILocalizationService>();
            //( sender as Button )!.Content = "Ginger";
            var dialog = new Settings()
            {
                DataContext = new SettingsViewModel(localizationService)
            };
            dialog.ShowDialog( this );

        }
        private static T GetService<T>() => Locator.Current.GetService<T>();
    }
}