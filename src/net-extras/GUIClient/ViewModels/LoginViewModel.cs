using System.Dynamic;using GUIClient.Services;
using Microsoft.Extensions.Localization;

namespace GUIClient.ViewModels;

public class LoginViewModel : ViewModelBase
{
    private ILocalizationService _localizationService;
    public IStringLocalizer _localizer;

    public string StrNotAccepted { get; }
    
    public string StrLogin { get; }
    
    public string StrUsername { get; }
    public string StrPassword { get; }

    public LoginViewModel(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
        _localizer = _localizationService.GetLocalizer();
        StrNotAccepted = _localizer["NotAccepted"];
        StrLogin = _localizer["Login"];
        StrPassword = _localizer["Password"];
        StrUsername = _localizer["Username"];
    }

    public bool IsAccepted { get; set; }

    public string Username { get; set;}
    public string Password { get; set; }
    
}