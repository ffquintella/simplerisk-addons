using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using AvaloniaEdit;
using GUIClient.Services;
using Microsoft.Extensions.Localization;
using Splat;
using LiveChartsCore;
using LiveChartsCore.Measure;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Model.Statistics;
using ReactiveUI;
using SkiaSharp;

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
    
    private ObservableCollection<ISeries>? _controlRisks;
    public ObservableCollection<ISeries>? ControlRisks
    {
        get => _controlRisks; 
        set => this.RaiseAndSetIfChanged(ref _controlRisks, value); 
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

    private void UpdateData()
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

        var totalMaturity = securityControlsStatistics.FameworkStats.Select(s => s.TotalMaturity).ToList();
        var totalDesiredMaturity = securityControlsStatistics.FameworkStats.Select(s => s.TotalDesiredMaturity - s.TotalMaturity).ToList();
        
        FrameworkControls = new ObservableCollection<ISeries>
        {
            new StackedColumnSeries<int>
            {
                Values = totalMaturity,
                Name = Localizer["Maturity"],
                Stroke = null,
                DataLabelsPaint = new SolidColorPaint(new SKColor(45, 45, 45)),
                DataLabelsSize = 14,
                DataLabelsPosition = DataLabelsPosition.Middle
            },
            new StackedColumnSeries<int>
            {
                Values = totalDesiredMaturity,
                Name = Localizer["DesiredMaturity"],  
                Stroke = null,
                DataLabelsPaint = new SolidColorPaint(new SKColor(45, 5, 5)),
                DataLabelsSize = 14,
                DataLabelsPosition = DataLabelsPosition.Middle
            },
        };
        
        FrameworkControlsXAxis = new List<Axis>
        {
            new Axis
            {
                Labels = securityControlsStatistics.FameworkStats.Select(s => s.Framework).ToList(),
                TextSize = 9,
                LabelsRotation = 0,
           }
        };
        
        var controlRisks = securityControlsStatistics.SecurityControls
            .Select(sc => new {sc.ControlName, sc.TotalRisk}).ToList();

        ControlRisks = new ObservableCollection<ISeries>();

        foreach (var controlRisk in controlRisks)
        {
            ControlRisks.Add(new PieSeries<double>
            {
                Values = new double[] {controlRisk.TotalRisk},
                Name = controlRisk.ControlName,
                DataLabelsPosition = PolarLabelsPosition.Outer,
                DataLabelsFormatter = p => $"{p.PrimaryValue} / {p.StackedValue!.Total} ({p.StackedValue.Share:P2})"
            });
        }
        

        
        /*ControlRisks = controlRisks.AsLiveChartsPieSeries((value, series) =>
        {
            // here you can configure the series assigned to each value.
            series.Name = $"Series for value {value}";
            series.DataLabelsPaint = new SolidColorPaint(new SKColor(30, 30, 30));
            series.DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Outer;
            series.DataLabelsFormatter = p => $"{p.PrimaryValue} / {p.StackedValue!.Total} ({p.StackedValue.Share:P2})";
        });*/
    }
    
    public void Initialize()
    {
        if (!_initialized)
        {
            UpdateData();
            _initialized = true;
        }
    }
    
}