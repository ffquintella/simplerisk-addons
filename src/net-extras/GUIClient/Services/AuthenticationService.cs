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

namespace GUIClient.Services;

public class AuthenticationService: IAuthenticationService
{
    public bool IsAuthenticated { get; set; } = false;

    private ILogger<AuthenticationService> _logger;
    private IRegistrationService _registrationService;
    private IRestService _restService;
    
    public AuthenticationCredential AuthenticationCredential { get; set; }

    public AuthenticationService(ILoggerFactory loggerFactory, 
        IRegistrationService registrationService,
        IRestService restService)
    {
        AuthenticationCredential = new AuthenticationCredential
        {
            AuthenticationType = AuthenticationType.None
        };
        
        _restService = restService;
        _logger = loggerFactory.CreateLogger<AuthenticationService>();
        _registrationService = registrationService;
    }
    
    public void TryAuthenticate(Window parentWindow)
    {
        var dialog = new Login();
        dialog.ShowDialog( parentWindow );
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