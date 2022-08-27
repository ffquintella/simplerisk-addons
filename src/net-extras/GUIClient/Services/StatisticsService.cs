using System;
using System.Collections.Generic;
using Model.Authentication;
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

            return response;
            
        }
        catch (Exception ex)
        {
            _logger.Error("Error getting user info {ExMessage}", ex.Message);
            return null;
        }
        
    }
    
   
}