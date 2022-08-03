using GUIClient.Services;
using Microsoft.Extensions.Localization;

namespace GUIClient.ViewModels;

public class SettingsViewModel: ViewModelBase
{
    private ILocalizationService _localizationService;
    public IStringLocalizer _localizer;

    public string StrServer { get; }
    public string StrSystem { get; }
    public string StrOperarionalSystem { get; }
    
    public SettingsViewModel(ILocalizationService localizationService)
    {
       _localizationService = localizationService;
       _localizer = _localizationService.GetLocalizer();

       StrSystem = _localizer["Sys"];
       StrServer = _localizer["Server"];
       StrOperarionalSystem = _localizer["OperationalSystem"];


    }

}