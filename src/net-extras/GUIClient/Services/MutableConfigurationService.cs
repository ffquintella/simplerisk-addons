using System.IO;

namespace GUIClient.Services;

public class MutableConfigurationService: IMutableConfigurationService
{
    private IEnvironmentService? _environmentService;
    public MutableConfigurationService(IEnvironmentService? environmentService = null)
    {
        _environmentService = environmentService;
    }

    public bool IsInitialized
    {
        get
        {
            if (_environmentService == null) return false;
            var configurationFilePath = _environmentService.ApplicationDataFolder + @"\configuration.db";
            if(File.Exists(configurationFilePath )) return true;
            return false;
        }
    }
}