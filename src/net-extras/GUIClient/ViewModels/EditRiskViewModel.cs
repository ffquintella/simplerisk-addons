using DAL.Entities;
using GUIClient.Models;

namespace GUIClient.ViewModels;

public class EditRiskViewModel: ViewModelBase
{
    public string StrRisk { get; }
    public string StrOperation { get; }
    public string StrOperationType { get; }
    
    public bool ShowEditFields { get; }

    private OperationType _operationType;
    
    public EditRiskViewModel(OperationType operation)
    {
        _operationType = operation;
        StrRisk = Localizer["Risk"];
        StrOperation = Localizer["Operation"] + ": ";
        StrOperationType = _operationType == OperationType.Create ? Localizer["Creation"] : Localizer["Edit"];
        if (_operationType == OperationType.Create)
        {
            Risk = new Risk();
            ShowEditFields = false;
        }
        else
        {
            ShowEditFields = true;
        }
    }
    
    public Risk Risk { get; set; }
}