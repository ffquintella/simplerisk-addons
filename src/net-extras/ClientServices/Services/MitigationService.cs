using System.Net;
using ClientServices.Interfaces;
using DAL.Entities;
using Model.Exceptions;
using RestSharp;

namespace ClientServices.Services;

public class MitigationService: ServiceBase, IMitigationService
{

    private IAuthenticationService _authenticationService;
    
    public MitigationService(IRestService restService, 
        IAuthenticationService authenticationService): base(restService)
    {
        _authenticationService = authenticationService;
    }

    public List<Team>? GetTeamsById(int id)
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest($"/Mitigations/{id}/Teams");
        try
        {
            var response = client.Get<List<Team>>(request);
            if (response == null)
            {
                _logger.Error("Error getting teams for mitigation {Id}", id);
                throw new RestComunicationException($"Error getting teams for mitigation {id}");
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting teams for mitigation  message:{Message}", ex.Message);
            throw new RestComunicationException("Error getting teams for mitigation", ex);
        }
    }
    
    public Mitigation? GetByRiskId(int id)
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest($"/Risks/{id}/Mitigation");
        try
        {
            var response = client.Get<Mitigation>(request);

            if (response == null)
            {
                _logger.Error("Error getting mitigation for risk {Id}", id);
                throw new RestComunicationException($"Error getting mitigation for risk {id}");
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting mitigation  message:{Message}", ex.Message);
            throw new RestComunicationException("Error getting risk mitigation", ex);
        }
    }

    public List<PlanningStrategy>? GetStrategies()
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest($"/Mitigations/Strategies");
        try
        {
            var response = client.Get<List<PlanningStrategy>>(request);

            if (response == null)
            {
                _logger.Error("Error getting mitigation strategies");
                throw new RestComunicationException($"Error getting mitigation strategies");
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting mitigation strategies");
            throw new RestComunicationException("Error getting mitigation strategies", ex);
        }
    }

    public List<MitigationCost>? GetCosts()
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest($"/Mitigations/Costs");
        try
        {
            var response = client.Get<List<MitigationCost>>(request);

            if (response == null)
            {
                _logger.Error("Error getting mitigation costs");
                throw new RestComunicationException($"Error getting mitigation costs");
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting mitigation costs");
            throw new RestComunicationException("Error getting mitigation costs", ex);
        } 
    }

    public List<MitigationEffort>? GetEfforts()
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest($"/Mitigations/Efforts");
        try
        {
            var response = client.Get<List<MitigationEffort>>(request);

            if (response == null)
            {
                _logger.Error("Error getting mitigation efforts");
                throw new RestComunicationException($"Error getting mitigation efforts");
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting mitigation efforts");
            throw new RestComunicationException("Error getting mitigation efforts", ex);
        } 
    }
    
    public Mitigation? GetById(int id)
    {
        using var client = _restService.GetClient();
        
        var request = new RestRequest($"/Mitigations/{id}");
        try
        {
            var response = client.Get<Mitigation>(request);

            if (response == null)
            {
                _logger.Error("Error getting mitigation");
                throw new RestComunicationException($"Error getting mitigation");
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error getting mitigation");
            throw new RestComunicationException("Error getting mitigation", ex);
        } 
    }

    public Mitigation? Create(Mitigation mitigation)
    {
        using var client = _restService.GetClient();
        
        var request = new RestRequest($"/Mitigations");

        request.AddJsonBody(mitigation);
        
        try
        {
            var response = client.Post<Mitigation>(request);

            if (response == null)
            {
                _logger.Error("Error creating mitigation");
                throw new RestComunicationException($"Error creating mitigation");
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error creating mitigation");
            throw new RestComunicationException("Error creating mitigation", ex);
        } 
    }

    public void DeleteTeamsAssociations(int mitigationId)
    {
        using var client = _restService.GetClient();
        
        var request = new RestRequest($"/Mitigations/{mitigationId}/Teams");

        try
        {
            var response = client.Delete(request);

            if (response == null)
            {
                _logger.Error("Error deleting mitigation associations");
                throw new RestComunicationException($"Error deleting mitigation associations");
            }
            
            if(response.StatusCode != HttpStatusCode.OK)
            {
                _logger.Error("Error deleting mitigation associations");
                throw new RestComunicationException($"Error deleting mitigation associations");
            }
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error deleting mitigation associations");
            throw new RestComunicationException("Error deleting mitigation associations", ex);
        } 
    }

    public void AssociateMitigationToTeam(int mitigationId, int teamId)
    {
        using var client = _restService.GetClient();
        
        var request = new RestRequest($"/Mitigations/{mitigationId}/Teams/Associate/{teamId}");

        try
        {
            var response = client.Get(request);

            if (response == null)
            {
                _logger.Error("Error associating mitigation to team");
                throw new RestComunicationException($"Error associating mitigation to team");
            }
            
            if(response.StatusCode != HttpStatusCode.OK)
            {
                _logger.Error("Error associating mitigation to team");
                throw new RestComunicationException($"Error associating mitigation to team");
            }
            
        }
        catch (HttpRequestException ex)
        {
            if (ex.StatusCode == HttpStatusCode.Unauthorized)
            {
                _authenticationService.DiscardAuthenticationToken();
            }
            _logger.Error("Error associating mitigation to team");
            throw new RestComunicationException("Error associating mitigation to team", ex);
        } 
    }
    
}