using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;
using RestSharp;
using Serilog;

namespace GUIClient.Services;

public class AssessmentsService: ServiceBase, IAssessmentsService
{
    public AssessmentsService(IRestService restService) : base(restService)
    {
        
    }
    
    public List<Assessment>? GetAssessments()
    {
        var client = _restService.GetClient();
        var request = new RestRequest("/Assessments");

        try
        {
            var response = client.Get<List<Assessment>>(request);
            return response;
        }
        catch (Exception ex)
        {
            _logger.Error("Error getting assessments: {0}", ex.Message);
            return null;
        }
        
    }

    public List<AssessmentQuestion>? GetAssessmentQuestions(int assessmentId)
    {
        var client = _restService.GetClient();
        var request = new RestRequest($"/Assessments/{assessmentId}/Questions");

        try
        {
            var response = client.Get<List<AssessmentQuestion>>(request);
            return response;
        }
        catch (Exception ex)
        {
            _logger.Error("Error getting assessments questions: {0}", ex.Message);
            return null;
        }
    }
    
    public List<AssessmentAnswer>? GetAssessmentAnswers(int assessmentId)
    {
        var client = _restService.GetClient();
        var request = new RestRequest($"/Assessments/{assessmentId}/Answers");

        try
        {
            var response = client.Get<List<AssessmentAnswer>>(request);
            return response;
        }
        catch (Exception ex)
        {
            _logger.Error("Error getting assessments answers: {0}", ex.Message);
            return null;
        }
    }

}