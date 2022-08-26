using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
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
using System.Text.Json;

namespace GUIClient.Services;

public class AuthenticationService: IAuthenticationService
{
    public bool IsAuthenticated { get; set; } = false;

    private ILogger<AuthenticationService> _logger;
    private IRegistrationService _registrationService;
    private IRestService _restService;
    private IMutableConfigurationService _mutableConfigurationService;
    
    public AuthenticationCredential AuthenticationCredential { get; set; }
    public AuthenticatedUserInfo? AuthenticatedUserInfo { get; set; }

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
        var isauth = _mutableConfigurationService.GetConfigurationValue("IsAuthenticate");
        var token = _mutableConfigurationService.GetConfigurationValue("AuthToken");
        
        if (isauth == "true" && CheckTokenValidTime(token!))
        {
            AuthenticationCredential.AuthenticationType = AuthenticationType.JWT;
            AuthenticationCredential.JWTToken = token;
            IsAuthenticated = true;
            AuthenticatedUserInfo = _mutableConfigurationService.GetAuthenticatedUser()!;
        }
        else
        {
            var dialog = new Login();
            dialog.ShowDialog( parentWindow );
        }
    }

    public bool CheckTokenValidTime(string token, int minutesToExpire = 0)
    {
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken.ValidTo > DateTime.UtcNow.AddMinutes(minutesToExpire)) return true;

        return false;
    }

    public int RefreshToken()
    {
        var client = _restService.GetClient();
        var request = new RestRequest("/Authentication/GetToken");

        try
        {
            var response = client.Get(request);

            if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
            {
                var token = JsonSerializer.Deserialize<string>(response.Content);
                //var token = response.Content;
                _mutableConfigurationService.SetConfigurationValue("IsAuthenticate", "true");
                _mutableConfigurationService.SetConfigurationValue("AuthToken", token);
                _mutableConfigurationService.SetConfigurationValue("AuthTokenTime", DateTime.Now.Ticks.ToString());
                AuthenticationCredential.AuthenticationType = AuthenticationType.JWT;
                AuthenticationCredential.JWTToken = token;
                IsAuthenticated = true;
                GetAuthenticatedUserInfo();
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


    /// <summary>
    /// Executes authentication against the server.
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <returns> 0 if success; -1 if unkown error; 1 if authentication error</returns>
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
                var token = JsonSerializer.Deserialize<string>(response.Content);
                //var token = response.Content;
                _mutableConfigurationService.SetConfigurationValue("IsAuthenticate", "true");
                _mutableConfigurationService.SetConfigurationValue("AuthToken", token);
                _mutableConfigurationService.SetConfigurationValue("AuthTokenTime", DateTime.Now.Ticks.ToString());
                AuthenticationCredential.AuthenticationType = AuthenticationType.JWT;
                AuthenticationCredential.JWTToken = token;
                IsAuthenticated = true;
                GetAuthenticatedUserInfo();
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

    public int GetAuthenticatedUserInfo()
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest("/Authentication/AuthenticatedUserInfo");
        
        try
        {
            var response = client.Get<AuthenticatedUserInfo>(request);

            if (response != null)
            {
                AuthenticatedUserInfo = response;
                _mutableConfigurationService.SaveAuthenticatedUser(AuthenticatedUserInfo);
                return 0;
            }

            return 1;
        }
        catch (Exception ex)
        {
            _logger.LogError("Error getting user info {ExMessage}", ex.Message);
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