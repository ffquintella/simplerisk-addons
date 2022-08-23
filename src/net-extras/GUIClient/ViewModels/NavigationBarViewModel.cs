using Avalonia.Controls;
using GUIClient.Configuration;
using GUIClient.Services;
using GUIClient.Views;

namespace GUIClient.ViewModels;

public class NavigationBarViewModel: ViewModelBase
{

    private ILocalizationService _localization;
    private ServerConfiguration _configuration;
    public NavigationBarViewModel(ILocalizationService localization,
        ServerConfiguration configuration)
    {
        _localization = localization;
        _configuration = configuration;
    }
    
    public void OnSettingsCommand(NavigationBar? parentControl)
    {
        
            
        var dialog = new Settings()
        {
            DataContext = new SettingsViewModel(_localization, _configuration)
        };
        dialog.ShowDialog( parentControl.ParentWindow );

    }
}