using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using GUIClient.Exceptions;
using Model.DTO;
using Model.Exceptions;
using RestSharp;

namespace GUIClient.Services;

public class UsersService: ServiceBase, IUsersService
{
    
    public UsersService(IRestService restService): base(restService)
    {
    }

    public string GetUserName(int id)
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest($"/Users/Name/{id}");
        
        try
        {
            var response = client.Get<string>(request);

            if (response == null)
            {
                _logger.Error("Error getting risks");
                response = "";
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            _logger.Error("Error getting user name message:{0}", ex.Message);
            throw new RestComunicationException("Error getting user name", ex);
        }
    }

    public List<UserListing> ListUsers()
    {
        var client = _restService.GetClient();
        
        var request = new RestRequest($"/Users/Listings");
        try
        {
            var response = client.Get<List<UserListing>>(request);

            if (response == null)
            {
                _logger.Error("Error listing users");
                throw new InvalidHttpRequestException("Error listing users", "/Users/Listings", "GET");
            }
            
            return response;
            
        }
        catch (HttpRequestException ex)
        {
            _logger.Error("Error listing users message:{0}", ex.Message);
            throw new RestComunicationException("Error listing users", ex);
        }
    }
}