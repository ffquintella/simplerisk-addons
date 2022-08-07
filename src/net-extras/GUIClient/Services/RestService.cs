using System;
using System.Net;
using GUIClient.Configuration;
using GUIClient.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using Splat;
using Locator = Splat.Locator;

namespace GUIClient.Services;

public class RestService: IRestService
{
    private IAuthenticationService _authenticationService;
    private ILogger<RestService> _logger;
    private ServerConfiguration _serverConfiguration;
    private bool _initialized = false;
    public RestService(ILoggerFactory loggerFactory, 
        ServerConfiguration serverConfiguration
        )
    {
        _logger = loggerFactory.CreateLogger<RestService>();
        _serverConfiguration = serverConfiguration;
    }

    private void Initialize()
    {
        if (_initialized) return;
        _initialized = true;
        _authenticationService = Locator.Current.GetService<IAuthenticationService>();
        ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
    }
    
    public RestClient GetClient()
    {
        Initialize();
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
    private static T GetService<T>() => Locator.Current.GetService<T>();
}