using GUIClient.Configuration;
using GUIClient.Services;
using Microsoft.Extensions.Localization;

namespace GUIClient.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private ILocalizationService _localizationService;
        public IStringLocalizer _localizer;
        
        public string StrApplicationMN { get; }
        public MainWindowViewModel(ILocalizationService localizationService)
        {
            _localizationService = localizationService;
            _localizer = _localizationService.GetLocalizer();

            StrApplicationMN = _localizer["ApplicationMN"];
        }

        public string Greeting => "Welcome to Avalonia!";
    }
}