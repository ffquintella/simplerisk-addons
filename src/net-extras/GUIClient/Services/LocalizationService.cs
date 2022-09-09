using System.Resources;
using GUIClient.Tools;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace GUIClient.Services;

public class LocalizationService: ILocalizationService
{

    private ILoggerFactory _loggerFactory;
    
    public LocalizationService(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }
    
    public IStringLocalizer GetLocalizer()
    {

        var localizer = new Locator();

        return localizer;
    }
    
    public ResourceManager GetResourceManager()
    {
        ResourceManager rm = new ResourceManager("GUIClient.Resources.Localization",
            typeof(LocalizationService).Assembly);
        
        return rm;
    }
}