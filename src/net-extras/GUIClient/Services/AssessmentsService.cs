using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;
using RestSharp;
using Serilog;

namespace GUIClient.Services;

public class AssessmentsService: ServiceBase, IAssessmentsService
{
    public AssessmentsService(RestService restService) : base(restService)
    {
        
    }
    
    public async Task<List<Assessment>?> GetAssessments()
    {
        var client = _restService.GetClient();
        var request = new RestRequest("/Assessments");

        try
        {
            var response = await client.GetAsync<List<Assessment>>(request);
            return response;
        }
        catch (Exception ex)
        {
            _logger.Error("Error getting assessments: {0}", ex.Message);
            return null;
        }
        
    }


}