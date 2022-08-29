using GUIClient.Services;
using Microsoft.Extensions.Localization;
using Splat;

namespace GUIClient.ViewModels;

public class AssessmentViewModel: ViewModelBase
{
    
    
    public string StrAssessments { get; }
    private bool _isInitialized = false;
    
    public AssessmentViewModel()
    {
        StrAssessments = Localizer["Assessments"];
        AuthenticationService.AuthenticationSucceeded += (obj, args) =>
        {
            Initialize();
        };
    }


    private void Initialize()
    {
        if (!_isInitialized)
        {
            _isInitialized = true;
        }
    }
    
    
}