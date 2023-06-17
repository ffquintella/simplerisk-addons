using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive;
using Avalonia.Controls;
using Avalonia.Media;
using ClientServices.Interfaces;
using GUIClient.Views;
using DAL.Entities;
using GUIClient.Models;
using MessageBox.Avalonia.DTO;
using MessageBox.Avalonia.Enums;
using Model.Risks;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class RiskViewModel: ViewModelBase
{

    
    #region LANGUAGE-STRINGS
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
    public string StrStatusFilter { get; }
    #endregion

    #region PROPERTIES
    private string _riskFilter = "";
    public string RiskFilter
    {
        get => _riskFilter;
        set
        {
            this.RaiseAndSetIfChanged(ref _riskFilter, value);
            ApplyFilter();
            //Risks = new ObservableCollection<Risk>(_allRisks!.Where(r => r.Subject.Contains(_riskFilter)));
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
            }
            this.RaiseAndSetIfChanged(ref _selectedRisk, value);
            //if(value != null && _hasDeleteRiskPermission) CanDeleteRisk = true;
        }
    }
    private ObservableCollection<Risk>? _allRisks;
    
    public ObservableCollection<Risk>? AllRisks
    {
        get => _allRisks;
        set
        {
            Risks = value;
            this.RaiseAndSetIfChanged(ref _allRisks, value);
        }
    }

    private ObservableCollection<Risk>? _risks;
    
    public ObservableCollection<Risk>? Risks
    {
        get => _risks;
        set => this.RaiseAndSetIfChanged(ref _risks, value);
    }

    private bool _hasDeleteRiskPermission;

    public bool CanDeleteRisk
    {
        get
        {
            //if (SelectedRisk == null) return false;
            return _hasDeleteRiskPermission;
        }
        set => this.RaiseAndSetIfChanged(ref _hasDeleteRiskPermission, value);
    }

    private IImmutableSolidColorBrush _newFilterColor = Brushes.DodgerBlue;
    public IImmutableSolidColorBrush NewFilterColor
    {
        get => _newFilterColor;
        set => this.RaiseAndSetIfChanged(ref _newFilterColor, value);
    }
    
    private IImmutableSolidColorBrush _mitigationFilterColor = Brushes.DodgerBlue;
    public IImmutableSolidColorBrush MitigationFilterColor
    {
        get => _mitigationFilterColor;
        set => this.RaiseAndSetIfChanged(ref _mitigationFilterColor, value);
    }
    
    private IImmutableSolidColorBrush _reviewFilterColor = Brushes.DodgerBlue;
    public IImmutableSolidColorBrush ReviewFilterColor
    {
        get => _reviewFilterColor;
        set => this.RaiseAndSetIfChanged(ref _reviewFilterColor, value);
    }
    
    private IImmutableSolidColorBrush _closedFilterColor = Brushes.White;
    public IImmutableSolidColorBrush ClosedFilterColor
    {
        get => _closedFilterColor;
        set => this.RaiseAndSetIfChanged(ref _closedFilterColor, value);
    }

    public ReactiveCommand<Window, Unit> BtAddRiskClicked { get; }
    public ReactiveCommand<Window, Unit> BtEditRiskClicked { get; }
    public ReactiveCommand<Unit, Unit> BtReloadRiskClicked { get; }
    public ReactiveCommand<Unit, Unit> BtDeleteRiskClicked { get; }
    
    public ReactiveCommand<Unit, Unit> BtNewFilterClicked { get; }
    public ReactiveCommand<Unit, Unit> BtMitigationFilterClicked { get; }
    public ReactiveCommand<Unit, Unit> BtReviewFilterClicked { get; }
    public ReactiveCommand<Unit, Unit> BtClosedFilterClicked { get; }
    #endregion

    public IRisksService _risksService;
    public IAuthenticationService _autenticationService;
    
    private bool _initialized;
    private List<RiskStatus> _filterStatuses;
    
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
        StrStatusFilter = Localizer["StatusFilter"] ;

        _risks = new ObservableCollection<Risk>();
        
        BtAddRiskClicked = ReactiveCommand.Create<Window>(ExecuteAddRisk);
        BtEditRiskClicked = ReactiveCommand.Create<Window>(ExecuteEditRisk);
        BtDeleteRiskClicked = ReactiveCommand.Create(ExecuteDeleteRisk);
        BtReloadRiskClicked = ReactiveCommand.Create(ExecuteReloadRisk);
        BtNewFilterClicked = ReactiveCommand.Create(ApplyNewFilter);
        BtMitigationFilterClicked = ReactiveCommand.Create(ApplyMitigationFilter);
        BtReviewFilterClicked = ReactiveCommand.Create(ApplyReviewFilter);
        BtClosedFilterClicked = ReactiveCommand.Create(ApplyClosedFilter);

        _risksService = GetService<IRisksService>();
        _autenticationService = GetService<IAuthenticationService>();

        _filterStatuses = new List<RiskStatus>()
        {
            RiskStatus.New,
            RiskStatus.ManagementReview,
            RiskStatus.MitigationPlanned
        };

        _autenticationService.AuthenticationSucceeded += (obj, args) =>
        {
            Initialize();
            
            if(_autenticationService.AuthenticatedUserInfo!.UserRole == "Admin" ||  
               _autenticationService.AuthenticatedUserInfo!.UserRole == "Administrator" || 
               _autenticationService.AuthenticatedUserInfo!.UserPermissions!.Any(p => p == "delete_risk"))
                CanDeleteRisk = true;
            
        };
        
    }

    private async void ApplyNewFilter()
    {
        if (_filterStatuses.Any(s => s == RiskStatus.New))
        {
            NewFilterColor = Brushes.White;
            _filterStatuses.Remove(RiskStatus.New);
            ApplyFilter();
        }
        else
        {
            NewFilterColor = Brushes.DodgerBlue;
            _filterStatuses.Add(RiskStatus.New);
            ApplyFilter();
        }
    }
    
    private async void ApplyMitigationFilter()
    {
        if (_filterStatuses.Any(s => s == RiskStatus.MitigationPlanned))
        {
            MitigationFilterColor = Brushes.White;
            _filterStatuses.Remove(RiskStatus.MitigationPlanned);
            ApplyFilter();
        }
        else
        {
            MitigationFilterColor = Brushes.DodgerBlue;
            _filterStatuses.Add(RiskStatus.MitigationPlanned);
            ApplyFilter();
        }
    }
    
    private async void ApplyReviewFilter()
    {
        if (_filterStatuses.Any(s => s == RiskStatus.ManagementReview))
        {
            ReviewFilterColor = Brushes.White;
            _filterStatuses.Remove(RiskStatus.ManagementReview);
            ApplyFilter();
        }
        else
        {
            ReviewFilterColor = Brushes.DodgerBlue;
            _filterStatuses.Add(RiskStatus.ManagementReview);
            ApplyFilter();
        }
    }
    
    private async void ApplyClosedFilter()
    {
        if (_filterStatuses.Any(s => s == RiskStatus.Closed))
        {
            ClosedFilterColor = Brushes.White;
            _filterStatuses.Remove(RiskStatus.Closed);
            LoadRisks(false);
            ApplyFilter();
        }
        else
        {
            ClosedFilterColor = Brushes.DodgerBlue;
            _filterStatuses.Add(RiskStatus.Closed);
            LoadRisks(true);
            ApplyFilter();
        }
    }

    private void ApplyFilter()
    {
        Risks = new ObservableCollection<Risk>(_allRisks!.Where(r => r.Subject.Contains(_riskFilter) && _filterStatuses.Any(s => r.Status == RiskHelper.GetRiskStatusName(s))));
        //Risks = new ObservableCollection<Risk>(_allRisks!.Where(r => r.Subject.Contains(_riskFilter)));
    }

    private async void ExecuteAddRisk(Window openWindow)
    {
        // OPENS a new window to create the risk
        
        var dialog = new EditRiskView()
        {
            DataContext = new EditRiskViewModel(OperationType.Create),
            WindowStartupLocation = WindowStartupLocation.CenterOwner,
            Width = 1000,
            Height = 650,
        };
        await dialog.ShowDialog( openWindow );
        AllRisks = new ObservableCollection<Risk>(_risksService.GetAllRisks());
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
        await dialog.ShowDialog( openWindow );
        AllRisks = new ObservableCollection<Risk>(_risksService.GetAllRisks());
    }
    private async void ExecuteDeleteRisk()
    {
        if (SelectedRisk == null)
        {
            var msgSelect = MessageBox.Avalonia.MessageBoxManager
                .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
                {
                    ContentTitle = Localizer["Error"],
                    ContentMessage = Localizer["SelectRiskDeleteMSG"] ,
                    Icon = Icon.Success,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                });

            await msgSelect.Show();
            return;
        }
        var messageBoxConfirm = MessageBox.Avalonia.MessageBoxManager
            .GetMessageBoxStandardWindow(   new MessageBoxStandardParams
            {
                ContentTitle = Localizer["Warning"],
                ContentMessage = Localizer["RiskDeleteConfirmationMSG"]  ,
                ButtonDefinitions = ButtonEnum.OkAbort,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Icon = Icon.Question,
            });
                        
        var confirmation = await messageBoxConfirm.Show();

        if (confirmation == ButtonResult.Ok)
        {
            _risksService.DeleteRisk(SelectedRisk);
            AllRisks = new ObservableCollection<Risk>(_risksService.GetAllRisks());
        }
    }
    
    private void LoadRisks(bool includeClosed = false)
    {
        AllRisks = new ObservableCollection<Risk>(_risksService.GetAllRisks(includeClosed));
    }
    
    private void ExecuteReloadRisk()
    {
        if(_filterStatuses.Any(s => s == RiskStatus.Closed))
            LoadRisks(true);
        else
            LoadRisks();
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