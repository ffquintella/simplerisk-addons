using System;
using System.Collections.Generic;
using System.Net;
using Avalonia.Controls;
using GUIClient.Models;
using GUIClient.ViewModels;
using GUIClient.Views;
using Microsoft.Extensions.Logging;
using Model.Authentication;
using RestSharp;
using RestSharp.Authenticators;
using Serilog;

namespace GUIClient.Services;

public class AuthenticationService: IAuthenticationService
{
    public bool IsAuthenticated { get; set; } = false;

    private ILogger<AuthenticationService> _logger;
    private IRegistrationService _registrationService;
    private IRestService _restService;
    private IMutableConfigurationService _mutableConfigurationService;
    
    public AuthenticationCredential AuthenticationCredential { get; set; }

    public AuthenticationService(ILoggerFactory loggerFactory, 
        IRegistrationService registrationService,
        IRestService restService,
        IMutableConfigurationService mutableConfigurationService)
    {
        AuthenticationCredential = new AuthenticationCredential
        {
            AuthenticationType = AuthenticationType.None
        };
        
        _restService = restService;
        _logger = loggerFactory.CreateLogger<AuthenticationService>();
        _registrationService = registrationService;
        _mutableConfigurationService = mutableConfigurationService;
    }
    
    public void TryAuthenticate(Window parentWindow)
    {
        var dialog = new Login();
        dialog.ShowDialog( parentWindow );
    }

    public int DoServerAuthentication(string user, string password)
    {
        var client = _restService.GetClient();
        var request = new RestRequest("/Authentication/GetToken");
        client.Authenticator = new HttpBasicAuthenticator(user, password);
        
        try
        {
            var response = client.Get(request);

            if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
            {
                var token = response.Content;
                _mutableConfigurationService.SetConfigurationValue("AuthUser", user);
                _mutableConfigurationService.SetConfigurationValue("AuthToken", token);
                _mutableConfigurationService.SetConfigurationValue("AuthTokenTime", DateTime.Now.Ticks.ToString());
                IsAuthenticated = true;
                return 0;
            }

            if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.NotFound)
            {
                Log.Error("Authentication Error response code: {0}", response.StatusCode);
                return 1;
            }
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            
        }
        
        return -1;
    }

    public List<AuthenticationMethod> GetAuthenticationMethods()
    {
        var defaultResponse = new List<AuthenticationMethod>();
        defaultResponse.Add(new AuthenticationMethod
        {
            Name= "Error",
            Type = "Basic"
        });
        
        var client = _restService.GetClient();
        
        var request = new RestRequest("/Authentication/AuthenticationMethods");
        try
        {
            var response = client.Get<List<AuthenticationMethod>>(request);

            if (response != null)
            {
                return response;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            
        }
        return defaultResponse;

    }
}