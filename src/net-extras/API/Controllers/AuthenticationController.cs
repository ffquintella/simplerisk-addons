using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Model.Authentication;
using ServerServices;

namespace API.Controllers;

[Authorize(Policy = "RequireValidUser")]
[ApiController]
[Route("[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly ILogger<AuthenticationController> _logger;
    private readonly IConfiguration _configuration;
    private readonly IEnvironmentService _environmentService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserManagementService _userManagementService;
    
    public AuthenticationController(ILogger<AuthenticationController> logger, 
        IConfiguration configuration,
        IEnvironmentService environmentService,
        IHttpContextAccessor httpContextAccessor,
        IUserManagementService userManagementService)
    {
        _logger = logger;
        _configuration = configuration;
        _environmentService = environmentService;
        _httpContextAccessor = httpContextAccessor;
        _userManagementService = userManagementService;
    }

    [HttpGet]
    [Route("GetToken")]
    public ActionResult<string> GetToken()
    {
        var symmetricKey = Convert.FromBase64String(_environmentService.ServerSecretToken);
        var tokenHandler = new JwtSecurityTokenHandler();

        var now = DateTime.UtcNow;
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, _httpContextAccessor.HttpContext!.User!.Identity!.Name!)
            }),

            Expires = now.AddMinutes(Convert.ToInt32(_configuration["JWT:Timeout"])),
        
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(symmetricKey), 
                SecurityAlgorithms.HmacSha256Signature)
        };

        var stoken = tokenHandler.CreateToken(tokenDescriptor);
        var token = tokenHandler.WriteToken(stoken);

        return token;
    }


    [HttpGet]
    [Route("AuthenticatedUserInfo")]
    public AuthenticatedUserInfo GetAuthenticatedUserInfo()
    {
        var userAccount = _httpContextAccessor.HttpContext!.User!.Identity!.Name!;
        
        if (userAccount == null)
        {
            _logger.LogError("Authenticated userAccount not found");
            throw new UserNotFoundException();
        }

        var user = _userManagementService.GetUser(userAccount);
        if (user == null)
        {
            _logger.LogError("Authenticated user not found");
            throw new UserNotFoundException();
        }
        
        var info = new AuthenticatedUserInfo
        {  
            UserAccount = userAccount,
            UserName = user.Name,
            UserEmail = Encoding.UTF8.GetString(user.Email)
        };

        return info;
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