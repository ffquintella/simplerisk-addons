using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Registration;
using Serilog;
using ServerServices;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class RegistrationController : ControllerBase
{
    private IClientRegistrationService _clientRegistrationService;

    
    public RegistrationController(IClientRegistrationService clientRegistrationService)
    {
        _clientRegistrationService = clientRegistrationService;
    }
    
    
    [AllowAnonymous]
    [HttpPost]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status412PreconditionFailed, Type = typeof(string))]
    public ActionResult<string> Register([FromBody] RegistrationRequest request)
    {
        string hashCode = String.Format("{0:X}", request.Id.GetHashCode());

        var newRequest = new AddonsClientRegistration
        {
            Hostname = request.Hostname,
            ExternalId = request.Id,
            LastVerificationDate = DateTime.Now,
            LoggedAccount = request.LoggedAccount,
            Name = hashCode,
            RegistrationDate = DateTime.Now,
            Status = "requested"
        };
        

        try
        {
            var result = _clientRegistrationService.Add(newRequest);
            if(result == 0) return Ok(hashCode);
            if(result == 1) return StatusCode(StatusCodes.Status412PreconditionFailed, "Already exists");
        }
        catch (Exception ex)
        {
            StatusCode(StatusCodes.Status503ServiceUnavailable, "Internal Error msg: " + ex.Message);
        }
        Log.Error("Unknown error detected on request. This point should not be reached - rc.01");
        return StatusCode(StatusCodes.Status500InternalServerError, "Unknown error");
        
        
    }
}