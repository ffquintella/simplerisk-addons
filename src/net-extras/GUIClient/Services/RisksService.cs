using System.Collections.Generic;
using DAL.Entities;

namespace GUIClient.Services;

public class RisksService: IRisksService
{
    private IRestService _restService;
    
    
    public RisksService(IRestService restService)
    {
        _restService = restService;
    }
    
    public List<Risk> GetAllRisks()
    {
        var result = new List<Risk>();

        return result;
    }
}