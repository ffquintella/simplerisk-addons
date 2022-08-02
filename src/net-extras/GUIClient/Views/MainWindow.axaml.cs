using Avalonia.Controls;
using Avalonia.Interactivity;
using GUIClient.ViewModels;

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
            //( sender as Button )!.Content = "Ginger";
            var dialog = new Settings()
            {
                DataContext = new SettingsViewModel()
            };
            dialog.ShowDialog( this );

        }
    }
}