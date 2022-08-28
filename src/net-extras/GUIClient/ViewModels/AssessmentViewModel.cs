using GUIClient.Services;
using Microsoft.Extensions.Localization;
using Splat;

namespace GUIClient.ViewModels;

public class AssessmentViewModel: ViewModelBase
{
    public IStringLocalizer _localizer;
    
    public string StrAssessments { get; } 
    
    public AssessmentViewModel()
    {
        var localizationService = GetService<ILocalizationService>();
        _localizer = localizationService.GetLocalizer();
        StrAssessments = _localizer["Assessments"];
    }
    private static T GetService<T>() => Locator.Current.GetService<T>();
}