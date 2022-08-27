using System.Collections.Generic;
using System.Collections.ObjectModel;
using GUIClient.Services;
using Microsoft.Extensions.Localization;
using Splat;

using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Model.Statistics;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class DashboardViewModel: ViewModelBase
{
    public IStringLocalizer _localizer;
    public IStatisticsService _statisticsService;
    public IAuthenticationService _authenticationService;
    
    private bool _initialized = false;
    
    private ObservableCollection<ISeries> _risksOverTime;

    public string StrWelcome { get; set; }

    public ISeries[] Series { get; set; } = new ISeries[]
    {
        new LineSeries<double>
        {
            Values = new double[] {2, 9, 3, 5, 3, 4, 6},
            Fill = null
        }
    };

    public ObservableCollection<ISeries> RisksOverTime
    {
        get => _risksOverTime;
        set => this.RaiseAndSetIfChanged(ref _risksOverTime, value);
    }
    
    public DashboardViewModel()
    {
        var localizationService = GetService<ILocalizationService>();
        _localizer = localizationService.GetLocalizer();
        _statisticsService = GetService<IStatisticsService>();
        _authenticationService = GetService<IAuthenticationService>();

        _authenticationService.AuthenticationSucceeded += (obj, args) =>
        {
            Initialize();
        };
        
        StrWelcome = _localizer["WelcomeMSG"];
    }
    
    public void Initialize()
    {
        if (!_initialized)
        {
            RisksOverTime = new ObservableCollection<ISeries>
            {
                new LineSeries<RisksOnDay>
                {
                    Name = "Risks Over Time",
                    Values = _statisticsService.GetRisksOverTime()
                }
            };
                    

            _initialized = true;
        }
    }
    private static T GetService<T>() => Locator.Current.GetService<T>();
}