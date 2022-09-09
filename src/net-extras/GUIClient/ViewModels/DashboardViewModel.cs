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

    private string StrControlStatistics { get; }

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

    private ObservableCollection<ISeries>? _frameworkControls;
    public ObservableCollection<ISeries>? FrameworkControls
    {
        get => _frameworkControls; 
        set => this.RaiseAndSetIfChanged(ref _frameworkControls, value); 
    }
    
    
    public List<Axis> _frameworkControlsXAxis;
    public List<Axis> _frameworkControlsYAxis;
    
    public List<Axis> FrameworkControlsXAxis
    {
        get => _frameworkControlsXAxis;
        set => this.RaiseAndSetIfChanged(ref _frameworkControlsXAxis, value);
    }
    
    public List<Axis> FrameworkControlsYAxis
    {
        get => _frameworkControlsYAxis;
        set => this.RaiseAndSetIfChanged(ref _frameworkControlsYAxis, value);
    }
    
    public DashboardViewModel()
    {
        _statisticsService = GetService<IStatisticsService>();

        _risksOverTimeXAxis = new List<Axis>();
        _frameworkControlsXAxis = new List<Axis>();
        _frameworkControlsYAxis = new List<Axis>();
        
        AuthenticationService.AuthenticationSucceeded += (obj, args) =>
        {
            Initialize();
        };
        
        StrWelcome = Localizer["WelcomeMSG"];
        StrRisksOverTime = Localizer["RisksOverTime"];
        StrControlStatistics = Localizer["ControlStatistics"];
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
                    MinLimit = 20,
                    MaxLimit = riskDays.Count 
                    
                }
            };
            
            // Security Control 
            var securityControlsStatistics = _statisticsService.GetSecurityControlStatistics();

            _initialized = true;
        }
    }
    
}