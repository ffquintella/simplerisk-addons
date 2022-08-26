using System;
using System.Collections.Generic;
using Model.Authentication;
using RestSharp;

namespace GUIClient.Services;

public class StatisticsService: ServiceBase, IStatisticsService 
{
    
    

    public StatisticsService(IRestService restService): base(restService)
    {
        
    }
    
    public Dictionary<string, double> GetRisksOverTime()
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest("/Statistics/RisksOverTime");
        
        try
        {
            var response = client.Get<Dictionary<string, double>>(request);

            return response;
            
        }
        catch (Exception ex)
        {
            _logger.Error("Error getting user info {ExMessage}", ex.Message);
            return null;
        }
        
    }
    
   
}