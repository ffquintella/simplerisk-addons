using GUIClient.Services;
using Microsoft.Extensions.Localization;
using Splat;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;

namespace GUIClient.ViewModels;

public class DashboardViewModel: ViewModelBase
{
    public IStringLocalizer _localizer;
    
    public string StrWelcome { get; set; }

    public ISeries[] Series { get; set; } = new ISeries[]
    {
        new LineSeries<double>
        {
            Values = new double[] {2, 9, 3, 5, 3, 4, 6},
            Fill = null
        }
    };

    public DashboardViewModel()
    {
        var localizationService = GetService<ILocalizationService>();
        _localizer = localizationService.GetLocalizer();
        
        StrWelcome = _localizer["WelcomeMSG"];
    }
    
    private static T GetService<T>() => Locator.Current.GetService<T>();
}