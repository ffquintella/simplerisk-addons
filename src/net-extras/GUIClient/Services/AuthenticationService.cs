using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Runtime.CompilerServices;
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

public class AuthenticationService: ServiceBase, IAuthenticationService, INotifyPropertyChanged
{
    //public bool IsAuthenticated { get; set; } = false;

    private bool _isAuthenticated = false;
    public bool IsAuthenticated
    {
        get
        {
            return _isAuthenticated;
        }

        set
        {
            if (value != _isAuthenticated)
            {
                _isAuthenticated = value;
                NotifyPropertyChanged();
            }
        }
    }

    
    
    private IRegistrationService _registrationService;
    private IMutableConfigurationService _mutableConfigurationService;
    public AuthenticationCredential AuthenticationCredential { get; set; }
    public AuthenticatedUserInfo? AuthenticatedUserInfo { get; set; }

    public AuthenticationService( 
        IRegistrationService registrationService,
        IRestService restService,
        IMutableConfigurationService mutableConfigurationService): base(restService)
    {
        AuthenticationCredential = new AuthenticationCredential
        {
            AuthenticationType = AuthenticationType.None
        };
        
        _registrationService = registrationService;
        _mutableConfigurationService = mutableConfigurationService;
    }
    
    public void TryAuthenticate(Window parentWindow)
    {
        var isauth = _mutableConfigurationService.GetConfigurationValue("IsAuthenticate");
        var token = _mutableConfigurationService.GetConfigurationValue("AuthToken");
        
        if (isauth == "true" && CheckTokenValidTime(token!))
        {
            _logger.Debug("User is authenticated");
            AuthenticationCredential.AuthenticationType = AuthenticationType.JWT;
            AuthenticationCredential.JWTToken = token;
            IsAuthenticated = true;
            AuthenticatedUserInfo = _mutableConfigurationService.GetAuthenticatedUser()!;
        }
        else
        {
            _logger.Debug("Starting authentication");
            var dialog = new Login();
            dialog.ShowDialog( parentWindow );
        }
    }

    public bool CheckTokenValidTime(string token, int minutesToExpire = 0)
    {
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.ReadToken(token) as JwtSecurityToken;

        if (jwtToken.ValidTo > DateTime.UtcNow.AddMinutes(minutesToExpire))
        {
            _logger.Debug("Token is valid");
            return true;
        }
        else
        {
            _logger.Debug("Token is expired");
            return false;
        }
        
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
                _logger.Error("Authentication Error response code: {0}", response.StatusCode);
                return 1;
            }
            
        }
        catch (Exception ex)
        {
            _logger.Error("Unkown error {0}", ex.Message);
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
                _logger.Information("User {0} authenticated", user);
                return 0;
            }

            if (response.StatusCode == HttpStatusCode.Forbidden || response.StatusCode == HttpStatusCode.NotFound)
            {
                _logger.Error("Authentication Error response code: {0}", response.StatusCode);
                return 1;
            }
            
        }
        catch (Exception ex)
        {
            _logger.Error("Unkown error {0}", ex.Message);
            
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
            _logger.Error("Error getting user info {ExMessage}", ex.Message);
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
                _logger.Debug("Listing authentication methods");
                return response;
            }
        }
        catch (Exception ex)
        {
            _logger.Error("Unkown error {0}", ex.Message);
            
        }
        return defaultResponse;

    }

    // This method is called by the Set accessor of each property.
    // The CallerMemberName attribute that is applied to the optional propertyName
    // parameter causes the property name of the caller to be substituted as an argument.
    private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    public event PropertyChangedEventHandler? PropertyChanged;
}