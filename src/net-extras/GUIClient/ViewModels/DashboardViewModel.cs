using System.Collections.Generic;
using GUIClient.Services;
using Microsoft.Extensions.Localization;
using Splat;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class DashboardViewModel: ViewModelBase
{
    public IStringLocalizer _localizer;
    public IStatisticsService _statisticsService;
    public IAuthenticationService _authenticationService;
    
    private bool _initialized = false;
    
    private Dictionary<string, double> _risksOverTime;
    public Dictionary<string, double> RisksOverTime
    {
        get => _risksOverTime;
        set => this.RaiseAndSetIfChanged(ref _risksOverTime, value);
    }
    
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
        _statisticsService = GetService<IStatisticsService>();
        _authenticationService = GetService<IAuthenticationService>();

        _authenticationService.PropertyChanged += (obj, args) =>
        {
            Initialize();
        };
        
        StrWelcome = _localizer["WelcomeMSG"];
    }
    
    public void Initialize()
    {
        if (!_initialized)
        {
            RisksOverTime = _statisticsService.GetRisksOverTime();
            _initialized = true;
        }
    }
    private static T GetService<T>() => Locator.Current.GetService<T>();
}