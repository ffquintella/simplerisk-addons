using System;
using System.Net;
using GUIClient.Configuration;
using GUIClient.Models;
using GUIClient.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using RestSharp;
using RestSharp.Authenticators;
using Splat;
using Locator = Splat.Locator;

namespace GUIClient.Services;

public class RestService: IRestService
{
    private IAuthenticationService? _authenticationService;
    private ILogger<RestService> _logger;
    private ServerConfiguration _serverConfiguration;
    private bool _initialized = false;

    private RestClientOptions? _options;
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
        //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;
        _options = new RestClientOptions(_serverConfiguration.Url) {
            RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
        };
    }
    
    public RestClient GetClient()
    {
        Initialize();
        if (_authenticationService!.IsAuthenticated)
        {
            if (_authenticationService.AuthenticationCredential.AuthenticationType == AuthenticationType.JWT)
            {
                if(!_authenticationService.CheckTokenValidTime(_authenticationService.AuthenticationCredential.JWTToken,
                   60 * 5))
                   {
                       
                   }
                
                var client = new RestClient(_options!);
                client.Authenticator = new JwtAuthenticator(_authenticationService.AuthenticationCredential.JWTToken!);
                //client.AddDefaultHeader("Authorization", $"Jwt {_authenticationService.AuthenticationCredential.JWTToken}");
                
                
                return client;
            }
            throw new NotImplementedException();
        }
        else
        {
           
            var client = new RestClient(_options!);
            return client;
        }
        
    }
    private static T GetService<T>() => Locator.Current.GetService<T>();
}