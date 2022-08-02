using System.Resources;
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
        var options = Options.Create(new LocalizationOptions()
        {
            ResourcesPath = "Resources",
        });
       
        var factory = new ResourceManagerStringLocalizerFactory( options, _loggerFactory);
        
        var localizer = factory.Create(App.Current.GetType());

        return localizer;
    }
    
    public ResourceManager GetResourceManager()
    {
        ResourceManager rm = new ResourceManager("GUIClient.Resources.Localization",
            typeof(LocalizationService).Assembly);
        
        return rm;
    }
}