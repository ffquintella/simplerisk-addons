using GUIClient.Services;
using Microsoft.Extensions.Localization;

namespace GUIClient.ViewModels;

public class SettingsViewModel: ViewModelBase
{
    private ILocalizationService _localizationService;
    private IStringLocalizer _localizer;
    public SettingsViewModel(ILocalizationService localizationService)
    {
       _localizationService = localizationService;
       _localizer = _localizationService.GetLocalizer();
    }
    public string StrSystem => _localizer["Settings"];
}