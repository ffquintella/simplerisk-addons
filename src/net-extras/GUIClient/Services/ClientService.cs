using System;
using System.Collections.Generic;
using System.Net;
using Microsoft.Extensions.Logging;
using Model;
using Model.Exceptions;
using RestSharp;
using Serilog;
using ILogger = Serilog.ILogger;


namespace GUIClient.Services;

public class ClientService: IClientService
{
    
    private ILogger _logger;
    private IRestService _restService;
    public ClientService(ILogger logger,
        IRestService restService)
    {
        _logger = logger;
        _restService = restService;
    }

    public List<Client> GetAll()
    {
        var restClient = _restService.GetClient();
        var request = new RestRequest("/Clients");
        
        try
        {
            var response = restClient.Get<List<Client>>(request);

            return response;

        }
        catch (Exception ex)
        {
            _logger.Error("Error listing clients {ExMessage}", ex.Message);
            throw new RestComunicationException(ex.Message);
        }
    }

    public int Approve(int id)
    {
        var restClient = _restService.GetClient();
        var request = new RestRequest($"/Clients/{id}/approve");
        
        try
        {
            var response = restClient.Get(request);

            if (response.IsSuccessful && response.StatusCode == HttpStatusCode.OK)
            {
                return 0;
            }

            return -1;

        }
        catch (Exception ex)
        {
            _logger.Error("Error approving client {ExMessage}", ex.Message);
            throw new RestComunicationException(ex.Message);
        }
    }
}