using System.Reactive;
using DAL.Entities;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class RiskViewModel: ViewModelBase
{
    public string StrRisk { get; }
    
    
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
    public ReactiveCommand<Unit, Unit> BtDeleteRiskClicked { get; }
    public RiskViewModel() : base()
    {
        StrRisk = Localizer["Risk"];
        
        BtAddRiskClicked = ReactiveCommand.Create(ExecuteAddRisk);
        BtDeleteRiskClicked = ReactiveCommand.Create(ExecuteDeleteRisk);
    }
    
    private void ExecuteAddRisk()
    {

    }
    private void ExecuteDeleteRisk()
    {

    }
}