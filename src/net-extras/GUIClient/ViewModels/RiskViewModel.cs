using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using ClientServices.Interfaces;
using GUIClient.Views;
using DAL.Entities;
using GUIClient.Models;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class RiskViewModel: ViewModelBase
{
    private bool _initialized;
    
    public string StrRisk { get; }
    public string StrDetails { get; }
    public string StrSubject { get; }
    public string StrStatus { get; }
    public string StrSource { get; }
    public string StrCategory { get; }
    public string StrNotes { get; }
    public string StrOwner { get; }
    public string StrManager { get; }
    public string StrCreation { get; }
    public string StrSubmittedBy { get; }
    public string StrRiskType { get; }

    private string _riskFilter = "";
    public string RiskFilter
    {
        get => _riskFilter;
        set
        {
            this.RaiseAndSetIfChanged(ref _riskFilter, value);
            Risks = new ObservableCollection<Risk>(_allRisks.Where(r => r.Subject.Contains(_riskFilter)));
        }
    }
    
    private Hydrated.Risk? _hdRisk;
    public Hydrated.Risk? HdRisk
    {
        get { return _hdRisk; }
        set
        {
            this.RaiseAndSetIfChanged(ref _hdRisk, value);
        }
    }

    private Risk? _selectedRisk;

    public Risk? SelectedRisk
    {
        get
        {
            return _selectedRisk;
        }
        set
        {
            if (value != null)
            {
                HdRisk = new Hydrated.Risk(value);
            }
            else
            {
                HdRisk = null;
                //throw new Exception("Invalid selected Risk");
            }
            this.RaiseAndSetIfChanged(ref _selectedRisk, value);
        }
    }
    
    public ReactiveCommand<Window, Unit> BtAddRiskClicked { get; }
    public ReactiveCommand<Window, Unit> BtEditRiskClicked { get; }
    public ReactiveCommand<Unit, Unit> BtReloadRiskClicked { get; }
    public ReactiveCommand<Unit, Unit> BtDeleteRiskClicked { get; }
    
    public IRisksService _risksService;
    public IAuthenticationService _autenticationService;
    
    
    private ObservableCollection<Risk> _allRisks;
    
    public ObservableCollection<Risk> AllRisks
    {
        get => _allRisks;
        set
        {
            Risks = value;
            this.RaiseAndSetIfChanged(ref _allRisks, value);
        }
    }

    private ObservableCollection<Risk> _risks;
    
    public ObservableCollection<Risk> Risks
    {
        get => _risks;
        set => this.RaiseAndSetIfChanged(ref _risks, value);
    }
    
    public RiskViewModel()
    {
        StrRisk = Localizer["Risk"];
        StrDetails= Localizer["Details"];
        StrSubject = Localizer["Subject"] + ": ";
        StrStatus = Localizer["Status"] + ": ";
        StrSource = Localizer["Source"] + ": ";
        StrCategory = Localizer["Category"] + ": ";
        StrNotes = Localizer["Notes"] + ": ";
        StrOwner = Localizer["Owner"] + ":";
        StrManager = Localizer["Manager"] + ":";
        StrCreation = Localizer["Creation"] + ":";
        StrSubmittedBy = Localizer["SubmittedBy"] + ":";
        StrRiskType = Localizer["RiskType"] ;

        _risks = new ObservableCollection<Risk>();
        
        BtAddRiskClicked = ReactiveCommand.Create<Window>(ExecuteAddRisk);
        BtEditRiskClicked = ReactiveCommand.Create<Window>(ExecuteEditRisk);
        BtDeleteRiskClicked = ReactiveCommand.Create(ExecuteDeleteRisk);
        BtReloadRiskClicked = ReactiveCommand.Create(ExecuteReloadRisk);

        _risksService = GetService<IRisksService>();
        _autenticationService = GetService<IAuthenticationService>();
        
        _autenticationService.AuthenticationSucceeded += (obj, args) =>
        {
            Initialize();
        };
        
    }
    
    private void ExecuteAddRisk(Window openWindow)
    {
        // OPENS a new window to create the risk
        
        var dialog = new EditRiskView()
        {
            DataContext = new EditRiskViewModel(OperationType.Create),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Width = 1000,
            Height = 650,
        };
        dialog.ShowDialog( openWindow );
    }
    
    private async void ExecuteEditRisk(Window openWindow)
    {
        if (SelectedRisk == null)
        {
            var msgSelect = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
                {
                    ContentTitle = Localizer["Error"],
                    ContentMessage = Localizer["SelectRiskMSG"] ,
                    Icon = Icon.Success,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                });

            await msgSelect.Show();
            return;
        }
        
        // OPENS a new window to edit the risk

        var dialog = new EditRiskView()
        {
            DataContext = new EditRiskViewModel(OperationType.Edit, SelectedRisk),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Width = 1000,
            Height = 650,
        };
        dialog.ShowDialog( openWindow );
    }
    
    private void ExecuteDeleteRisk()
    {

    }
    private void ExecuteReloadRisk()
    {
        AllRisks = new ObservableCollection<Risk>(_risksService.GetAllRisks());
        RiskFilter = "";
    }

    private void Initialize()
    {
        if (!_initialized)
        {
            AllRisks = new ObservableCollection<Risk>(_risksService.GetAllRisks());
            
            _initialized = true;
        }
    }
}