using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    
    public IStatisticsService _statisticsService;
    
    
    private bool _initialized = false;
    
    private ObservableCollection<ISeries>? _risksOverTime;
    public List<Axis> _risksOverTimeXAxis;
    public string StrWelcome { get; }
    public string StrRisksOverTime { get;}
 

    public ObservableCollection<ISeries>? RisksOverTime
    {
        get => _risksOverTime;
        set => this.RaiseAndSetIfChanged(ref _risksOverTime, value);
    }
    
    public List<Axis> RisksOverTimeXAxis
    {
        get => _risksOverTimeXAxis;
        set => this.RaiseAndSetIfChanged(ref _risksOverTimeXAxis, value);
    }
    
    
    public DashboardViewModel()
    {
        _statisticsService = GetService<IStatisticsService>();

        _risksOverTimeXAxis = new List<Axis>();
        
        AuthenticationService.AuthenticationSucceeded += (obj, args) =>
        {
            Initialize();
        };
        
        StrWelcome = Localizer["WelcomeMSG"];
        StrRisksOverTime = Localizer["RisksOverTime"];
    }
    
    public void Initialize()
    {
        if (!_initialized)
        {
            var risksOverTimeValues = _statisticsService.GetRisksOverTime();
            var riskDays = risksOverTimeValues.Select(r => r.Day.ToShortDateString()).ToList();
            
            RisksOverTime = new ObservableCollection<ISeries>
            {
                new LineSeries<RisksOnDay>
                {
                    Name = "Risks Over Time",
                    Values = risksOverTimeValues
                }
            };

            RisksOverTimeXAxis = new List<Axis>
            {
                new Axis
                {
                    Labels = riskDays,
                    TextSize = 9,
                    LabelsRotation = 90,
                    MinLimit = 0,
                    MaxLimit = 10
                }
            };

            _initialized = true;
        }
    }
    
}