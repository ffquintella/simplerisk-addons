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
            ResourcesPath = "Resources/Localization",
        });
       
        var factory = new ResourceManagerStringLocalizerFactory( options, _loggerFactory);
        
        var localizer = factory.Create(App.Current.GetType());

        return localizer;
    }
}