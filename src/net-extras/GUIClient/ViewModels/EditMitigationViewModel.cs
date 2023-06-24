using DAL.Entities;
using GUIClient.Models;
using Model.Exceptions;

namespace GUIClient.ViewModels;

public class EditMitigationViewModel: ViewModelBase
{
    #region LANGUAGE

    public string StrMitigation { get; }
    public string StrSubmissionDate { get; }
    
    public string StrSolution { get; }
    public string StrPlannedDate { get; }
    
    public string StrPlanningStrategy { get; }
    
    public string StrSecurityRequirements { get; }
    
    public string StrMitigationEffort { get; }
    
    public string StrMitigationCost { get; }
    public string StrSecurityRecommendation { get; }
    
    public string StrMitigationOwner { get; }
    
    public string StrMitigationTeam { get; }
    
    public string StrMitigationPercent { get; }
    
    public string StrDocumentation { get; }
    
    public string StrSave { get; }
    public string StrCancel { get; }
    
    #endregion

    #region INTERNAL FIELDS

    private readonly OperationType _operationType;
    private Mitigation? _mitigation;

    #endregion

    public EditMitigationViewModel(OperationType operation, Mitigation? mitigation = null)
    {
        _operationType = operation;
        
        if (_operationType == OperationType.Edit && mitigation == null)
        {
            throw new InvalidParameterException("mitigation", "Mitigation cannot be null on edit operations");
        }
        _mitigation = _operationType == OperationType.Create ? new Mitigation() : mitigation;
        
        StrMitigation = Localizer["Mitigation"];
        StrSubmissionDate = Localizer["SubmissionDate"] + ":";
        StrSolution = Localizer["Solution"] + ":";
        StrPlannedDate = Localizer["PlannedDate"] + ":";
        StrPlanningStrategy = Localizer["PlanningStrategy"] + ":";
        StrSecurityRequirements = Localizer["SecurityRequirements"] + ":";
        StrMitigationEffort = Localizer["MitigationEffort"] + ":";
        StrMitigationCost = Localizer["MitigationCost"] + ":";
        StrSecurityRecommendation = Localizer["SecurityRecommendation"] + ":";
        StrMitigationOwner = Localizer["MitigationOwner"] + ":";
        StrMitigationTeam = Localizer["MitigationTeam"] + ":";
        StrMitigationPercent = Localizer["MitigationPercent"] + ":";
        StrDocumentation = Localizer["Documentation"] + ":";
        StrSave = Localizer["Save"];
        StrCancel = Localizer["Cancel"];
    }

    #region PROPERTIES

    

    #endregion
}