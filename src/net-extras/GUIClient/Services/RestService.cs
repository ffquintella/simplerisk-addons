using System;
using GUIClient.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;

namespace GUIClient.Services;

public class RestService: IRestService
{
    private IAuthenticationService _authenticationService;
    private ILogger<RestService> _logger;
    private IMutableConfigurationService _mutableConfigurationService;
    private ServerConfiguration _serverConfiguration;
    public RestService(ILoggerFactory loggerFactory, 
        IAuthenticationService authenticationService,
        IMutableConfigurationService mutableConfigurationService,
        ServerConfiguration serverConfiguration)
    {
        _logger = loggerFactory.CreateLogger<RestService>();
        _authenticationService = authenticationService;
        _mutableConfigurationService = mutableConfigurationService;
        _serverConfiguration = serverConfiguration;
    }

    public RestClient GetClient()
    {
        if (_authenticationService.IsAuthenticated)
        {
            throw new NotImplementedException();
        }
        else
        {
            var client = new RestClient(_serverConfiguration.Url);
            return client;
        }
        
    }
}