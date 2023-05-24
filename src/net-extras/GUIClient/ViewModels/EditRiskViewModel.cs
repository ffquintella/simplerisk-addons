namespace GUIClient.ViewModels;

public class EditRiskViewModel: ViewModelBase
{
    public string StrRisk { get; }
    
    EditRiskViewModel()
    {
        StrRisk = Localizer["Risk"];
    }
}