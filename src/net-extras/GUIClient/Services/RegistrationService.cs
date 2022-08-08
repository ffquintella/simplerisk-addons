using System;
using System.Globalization;
using System.Net;
using System.Resources;
using GUIClient.Models;
using GUIClient.Tools;
using Microsoft.Extensions.Logging;
using Model.Registration;
using RestSharp;

namespace GUIClient.Services;

public class RegistrationService: IRegistrationService
{
    private ILogger<RegistrationService> _logger;
    private IMutableConfigurationService _mutableConfigurationService;
    private IRestService _restService;

    
    public RegistrationService(ILoggerFactory loggerFactory, 
        IMutableConfigurationService mutableConfigurationService,
        IRestService restService
    )
    {
        _logger = loggerFactory.CreateLogger<RegistrationService>();
        _mutableConfigurationService = mutableConfigurationService;
        _restService = restService;

    }

    public bool IsRegistered
    {
        get
        {
            var isRegistredVal = _mutableConfigurationService.GetConfigurationValue("IsRegistered");
            return isRegistredVal == "true";
        }
    }


    public RegistrationSolicitationResult Register(string Id, bool force = false)
    {
        string hashCode = String.Format("{0:X}", Id.GetHashCode());
        if (!force)
        {
            if (IsRegistered)
                return new RegistrationSolicitationResult
                {
                    RequestID = hashCode,
                    Result = RequestResult.AlreadyExists
                };
        }

        var client = _restService.GetClient();

        var reqData = new RegistrationRequest
        {
            Id = Id,
            Hostname = ComputerInfo.GetComputerName(),
            LoggedAccount = ComputerInfo.GetLoggedUser()
            
        };
        
        var request = new RestRequest("Registration").AddJsonBody(reqData);
        var response = client.Post(request);

        RegistrationSolicitationResult result = null;
        
        if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
        {
            result = new RegistrationSolicitationResult
            {
                Result = RequestResult.Success,
                RequestID = response.Content
            };
            return result;
        }

        result = new RegistrationSolicitationResult
        {
            Result = RequestResult.Failure,
            RequestID = ""
        };
        return result;

    }
}