using System.Resources;
using Microsoft.Extensions.Logging;

namespace GUIClient.Services;

public class RegistrationService: IRegistrationService
{
    private ILogger<RegistrationService> _logger;
    private IMutableConfigurationService _mutableConfigurationService;
    
    public RegistrationService(ILoggerFactory loggerFactory, IMutableConfigurationService mutableConfigurationService)
    {
        _logger = loggerFactory.CreateLogger<RegistrationService>();
        _mutableConfigurationService = mutableConfigurationService;
    }
    
    public bool IsRegistered { get; }
    
    public void Register(string ID)
    {
        throw new System.NotImplementedException();
    }
}