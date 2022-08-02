using Avalonia.Controls;
using Avalonia.Interactivity;

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
            //var dialog = new Settings();
            //dialog.ShowDialog( this );
            
        }
    }
}