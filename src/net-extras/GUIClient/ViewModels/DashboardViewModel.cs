using GUIClient.Services;
using Microsoft.Extensions.Localization;
using Splat;

namespace GUIClient.ViewModels;

public class DashboardViewModel: ViewModelBase
{
    public IStringLocalizer _localizer;
    
    public string StrWelcome { get; set; } 
    
    public DashboardViewModel()
    {
        var localizationService = GetService<ILocalizationService>();
        _localizer = localizationService.GetLocalizer();
        
        StrWelcome = _localizer["WelcomeMSG"];
    }
    
    private static T GetService<T>() => Locator.Current.GetService<T>();
}