using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Authentication;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IConfiguration _configuration;
    
    public AuthenticationController(ILogger<AuthenticationController> logger, IConfiguration configuration)
    {
        _logger = logger;
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpGet]
    [Route("AuthenticationMethods")]
    public IEnumerable<AuthenticationMethod> GetAllAuthenticationMethods()
    {
        var result = new List<AuthenticationMethod>();

        var basic = new AuthenticationMethod
        {
            Name = "Simplerisk",
            Description = "Simplerisk Internal DB Authentication",
            Type = "Basic"
            
        };
        if (_configuration["Saml2:Enabled"] == "True")
        {
            var saml = new AuthenticationMethod
            {
                Name = "SAML",
                Description = "SAML Authentication",
                Type = "SAML"
            };
            result.Add(saml);

        }
        
        result.Add(basic);
        
        return result;
    }
    
    
}