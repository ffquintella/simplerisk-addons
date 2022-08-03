using GUIClient.Services;
using Microsoft.Extensions.Localization;

namespace GUIClient.ViewModels;

public class SettingsViewModel: ViewModelBase
{
    private ILocalizationService _localizationService;
    private IStringLocalizer _localizer;

    private string _strSystem;
    
    private string StrServer { get; }
    
    public SettingsViewModel(ILocalizationService localizationService)
    {
       _localizationService = localizationService;
       _localizer = _localizationService.GetLocalizer();

       _strSystem = _localizer["Sys"];
       StrServer = _localizer["Server"];


    }
    public string StrSystem
    {
        get { return _strSystem; }
    }
}