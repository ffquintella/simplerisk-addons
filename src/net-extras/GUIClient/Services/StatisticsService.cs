using System;
using System.Collections.Generic;
using Avalonia.Logging;
using Model.Authentication;
using Model.Exceptions;
using Model.Statistics;
using RestSharp;

namespace GUIClient.Services;

public class StatisticsService: ServiceBase, IStatisticsService 
{
    
    

    public StatisticsService(IRestService restService): base(restService)
    {
        
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
        catch (Exception ex)
        {
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