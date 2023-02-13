using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using DAL.Entities;
using Model.Exceptions;
using RestSharp;

namespace GUIClient.Services;

public class RisksService: ServiceBase, IRisksService
{
    private IAuthenticationService _authenticationService;

    public RisksService(IRestService restService, 
        IAuthenticationService authenticationService): base(restService)
    {
        _authenticationService = authenticationService;
    }
    
    public List<Risk> GetAllRisks()
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest("/Risks");
        
        try
        {
            var response = client.Get<List<Risk>>(request);

            if (response == null)
            {
                _logger.Error("Error getting risks");
                response = new List<Risk>();
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting all risks message:{0}", ex.Message);
            throw new RestComunicationException("Error getting all risks", ex);
        }
    }
    
    public List<Risk> GetUserRisks()
    {

        var client = _restService.GetClient();
        
        var request = new RestRequest("/Risks/MyRisks");
        
        try
        {
            var response = client.Get<List<Risk>>(request);

            if (response == null)
            {
                _logger.Error("Error getting my risks ");
                response = new List<Risk>();
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting my risks message:{0}", ex.Message);
            throw new RestComunicationException("Error getting my risks", ex);
        }


    }

    public string GetRiskCategory(int id)
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest($"/Risks/Category/{id}");
        
        try
        {
            var response = client.Get<Category>(request);

            if (response == null)
            {
                _logger.Error("Error getting category ");
            }
            
            return response.Name;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting risk category message:{0}", ex.Message);
            throw new RestComunicationException("Error getting risk category", ex);
        }
    }
}