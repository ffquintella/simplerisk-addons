﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using Avalonia.Logging;
using Model.Authentication;
using Model.DTO.Statistics;
using Model.Exceptions;
using Model.Statistics;
using RestSharp;

namespace GUIClient.Services;

public class StatisticsService: ServiceBase, IStatisticsService
{

    private IAuthenticationService _authenticationService;

    public StatisticsService(IRestService restService, IAuthenticationService authenticationService): base(restService)
    {
        _authenticationService = authenticationService;
    }
    
    public List<RisksOnDay> GetRisksOverTime()
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest("/Statistics/RisksOverTime");
        
        try
        {
            var response = client.Get<List<RisksOnDay>>(request);

            if (response == null)
            {
                _logger.Error("Error getting risks over time");
                response = new List<RisksOnDay>();
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting risks over time message:{0}", ex.Message);
            throw new RestComunicationException("Error getting risks over time", ex);
        }
        
    }

    public SecurityControlsStatistics GetSecurityControlStatistics()
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest("/Statistics/SecurityControls");
        
        try
        {
            var response = client.Get<SecurityControlsStatistics>(request);

            if (response == null)
            {
                _logger.Error("Error getting security control statistics");
                response = new SecurityControlsStatistics();
            }
            
            return response;
            
        }
        catch (Exception ex)
        {
            _logger.Error("Error getting security control statistics message:{0}", ex.Message);
            throw new RestComunicationException("Error getting risks over time", ex);
        }
        
    }
}