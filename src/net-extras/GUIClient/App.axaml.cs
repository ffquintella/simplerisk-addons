using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using GUIClient.Services;
using GUIClient.ViewModels;
using GUIClient.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Splat;


namespace GUIClient
{
    public partial class App : Application
    {

        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            
            var mutableConfigurationService = GetService<IMutableConfigurationService>();
            mutableConfigurationService.Initialize();
            
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow
                {
                    DataContext = new MainWindowViewModel(),
                };
            }

           
            base.OnFrameworkInitializationCompleted();
        }
        private static T GetService<T>() => Locator.Current.GetService<T>();
    }
}