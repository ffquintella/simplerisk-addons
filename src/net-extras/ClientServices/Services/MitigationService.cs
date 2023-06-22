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

    public Mitigation? GetById(int id)
    {
        throw new NotImplementedException();
    }
}