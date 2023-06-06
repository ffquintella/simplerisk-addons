using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Reflection.Metadata;
using DAL.Entities;
using Microsoft.AspNetCore.Http;
using Model.Exceptions;
using RestSharp;
using System.Text.Json;
using GUIClient.Models;

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
        
        var request = new RestRequest($"/Risks/Categories/{id}");
        
        try
        {
            var response = client.Get<Category>(request);

            if (response == null)
            {
                _logger.Error("Error getting category ");
                return "ERROR";
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
    
    public List<Category>? GetRiskCategories()
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest($"/Risks/Categories");
        
        try
        {
            var response = client.Get<List<Category>>(request);

            if (response == null)
            {
                _logger.Error("Error getting categories ");
                return null;
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting risk categories message:{0}", ex.Message);
            throw new RestComunicationException("Error getting risk categories", ex);
        }
    }
    
    public string GetRiskSource(int id)
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest($"/Risks/Sources/{id}");
        
        try
        {
            var response = client.Get<Source>(request);

            if (response == null)
            {
                _logger.Error("Error getting source ");
                return "ERROR";
            }
            
            return response.Name;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting risk source message:{0}", ex.Message);
            throw new RestComunicationException("Error getting risk source", ex);
        }
    }

    public Risk? CreateRisk(Risk risk)
    {
        risk.Id = 0;
        using (var client = _restService.GetClient())
        {
            var request = new RestRequest($"/Risks");

            request.AddJsonBody(risk);
        
            try
            {
                var response = client.Post(request);

                if (response.StatusCode != HttpStatusCode.Created)
                {
                    _logger.Error("Error creating risk ");
                    
                    var opResult = JsonSerializer.Deserialize<OperationError>(response!.Content!);
                    
                    return null;
                }
            
                return  JsonSerializer.Deserialize<Risk?>(response!.Content!);
            
            }
            catch (HttpRequestException ex)
            {
                if (ex.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _authenticationService.DiscardAuthenticationToken();
                }
                _logger.Error("Error creating risk message:{0}", ex.Message);
                throw new RestComunicationException("Error creating risk", ex);
            }
        }

    }
    
    public bool RiskSubjectExists(string? subject)
    {
        if (subject == null || subject == "") return false;
        
        var client = _restService.GetClient();
        
        var request = new RestRequest($"/Risks/Exists");

        request.AddParameter("subject", subject);
        
        try
        {
            var response = client.Get(request);

            if (response == null)
            {
                _logger.Error("Error getting risk subject status ");
                return false;
            }

            if (response.StatusCode == HttpStatusCode.OK ) return true;
            return false;

        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting risk subject status message:{0}", ex.Message);
            throw new RestComunicationException("Error getting risk subject", ex);
        }
        
    }
    
    public List<Source>? GetRiskSources()
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest($"/Risks/Sources");
        
        try
        {
            var response = client.Get<List<Source>>(request);

            if (response == null)
            {
                _logger.Error("Error getting sources ");
                return null;
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting risk source message:{0}", ex.Message);
            throw new RestComunicationException("Error getting risk source", ex);
        }
    }

    public List<RiskCatalog> GetRiskTypes()
    {
        return GetRiskTypes("");
    }
    public List<RiskCatalog> GetRiskTypes(string ids)
    {
        var all = false;
        if (ids == "") all = true;
        
        var client = _restService.GetClient();
        
        
        var request = new RestRequest($"/Risks/Catalogs");

        if(!all) request.AddParameter("list", ids);
        
        try
        {
            var response = client.Get<List<RiskCatalog>>(request);

            if (response == null)
            {
                _logger.Error("Error getting risk catalogs ");
                return new List<RiskCatalog>();
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting risk catalogs message:{0}", ex.Message);
            throw new RestComunicationException("Error getting risk catalogs", ex);
        }
    }
}