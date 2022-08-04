using System.Globalization;
using GUIClient.Configuration;
using GUIClient.Services;
using GUIClient.Tools;
using Microsoft.Extensions.Localization;

namespace GUIClient.ViewModels;

public class SettingsViewModel: ViewModelBase
{
    private ILocalizationService _localizationService;
    public IStringLocalizer _localizer;
    
    public string StrServer { get; }
    public string StrSystem { get; }
    public string StrOperationalSystem { get; }
    public string StrOperationalSystemData { get; }
    
    public string StrHost { get; }
    public string StrHostData { get; }
    
    public string ServerURL { get; }
    
    public SettingsViewModel(ILocalizationService localizationService, ServerConfiguration serverConfiguration)
    {
       _localizationService = localizationService;
       _localizer = _localizationService.GetLocalizer();

       StrSystem = _localizer["Sys"];
       StrServer = _localizer["Server"] ;
       StrOperationalSystem = _localizer["OperationalSystem"] + ":";
       StrHost = _localizer["Host"] +':';
       
       StrOperationalSystemData = ComputerInfo.GetOsVersion();
       StrHostData = ComputerInfo.GetComputerName() ;
       
       ServerURL = serverConfiguration.Url;

    }

}