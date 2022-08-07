using System;
using System.Resources;
using GUIClient.Models;
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
    
    public RegistrationSolicitationResult Register(string ID)
    {
        string hashCode = String.Format("{0:X}", ID.GetHashCode());

        var result = new RegistrationSolicitationResult
        {
            Result = RequestResult.Success,
            RequestID = hashCode
        };

        return result;

    }
}