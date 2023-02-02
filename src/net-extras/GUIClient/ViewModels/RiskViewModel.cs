using System.Collections.ObjectModel;
using System.Reactive;
using DAL.Entities;
using GUIClient.Services;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class RiskViewModel: ViewModelBase
{
    private bool _initialized = false;
    
    public string StrRisk { get; }
    public string StrDetails { get; }
    public string StrSubject { get; }
    
    public string StrStatus { get; }

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

                //UpdateAssessmentQuestionAnswers( value.Id);
            
            }
            this.RaiseAndSetIfChanged(ref _selectedRisk, value);
        }
    }
    
    public ReactiveCommand<Unit, Unit> BtAddRiskClicked { get; }
    
    public ReactiveCommand<Unit, Unit> BtReloadRiskClicked { get; }
    public ReactiveCommand<Unit, Unit> BtDeleteRiskClicked { get; }
    
    public IRisksService _risksService;
    public IAuthenticationService _autenticationService;
    
    
    private ObservableCollection<Risk> _risks;
    
    public ObservableCollection<Risk> Risks
    {
        get => _risks;
        set => this.RaiseAndSetIfChanged(ref _risks, value);
    }
    
    public RiskViewModel() : base()
    {
        StrRisk = Localizer["Risk"];
        StrDetails= Localizer["Details"];
        StrSubject= Localizer["Subject"] + ": ";
        StrStatus= Localizer["Status"] + ": ";
        
        
        BtAddRiskClicked = ReactiveCommand.Create(ExecuteAddRisk);
        BtDeleteRiskClicked = ReactiveCommand.Create(ExecuteDeleteRisk);
        BtReloadRiskClicked = ReactiveCommand.Create(ExecuteReloadRisk);

        _risksService = GetService<IRisksService>();
        _autenticationService = GetService<IAuthenticationService>();
        
        _autenticationService.AuthenticationSucceeded += (obj, args) =>
        {
            Initialize();
        };
        
    }
    
    private void ExecuteAddRisk()
    {

    }
    private void ExecuteDeleteRisk()
    {

    }
    private void ExecuteReloadRisk()
    {
        Risks = new ObservableCollection<Risk>(_risksService.GetAllRisks());
    }

    private void Initialize()
    {
        if (!_initialized)
        {
            Risks = new ObservableCollection<Risk>(_risksService.GetAllRisks());
            
            _initialized = true;
        }
    }
}