using GUIClient.Services;
using Microsoft.Extensions.Localization;

namespace GUIClient.ViewModels;

public class LoginViewModel: ViewModelBase
{
    private ILocalizationService _localizationService;
    public IStringLocalizer _localizer;
    
    public string StrNotAccepted { get; }
    public LoginViewModel(ILocalizationService localizationService)
    {
        _localizationService = localizationService ;
        _localizer = _localizationService.GetLocalizer();
        StrNotAccepted = _localizer["NotAccepted"];
    }
    public bool IsAccepted { get; set; }
}