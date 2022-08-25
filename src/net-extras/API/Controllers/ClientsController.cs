using AutoMapper;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;
using ServerServices;
using ILogger = Serilog.ILogger;

namespace API.Controllers;

[Authorize(Policy = "RequireAdminOnly")]
[ApiController]
[Route("[controller]")]
public class ClientsController : ControllerBase
{

    private IClientRegistrationService _clientRegistrationService;
    private ILogger _logger;
    private IMapper _mapper;
    
    public ClientsController(
        IClientRegistrationService clientRegistrationService, 
        ILogger logger,
        IMapper mapper)
    {
        _clientRegistrationService = clientRegistrationService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AddonsClientRegistration>))]
    public ActionResult<List<Client>> GetAll()
    {
        var clientsRegs = _clientRegistrationService.GetAll();
        var clients = _mapper.Map<List<Client>>(clientsRegs);
        
        
        return clients;
    }

}