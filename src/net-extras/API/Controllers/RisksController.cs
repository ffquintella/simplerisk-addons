using API.Tools;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Exceptions;
using ServerServices;
using ILogger = Serilog.ILogger;

namespace API.Controllers;

[Authorize(Policy = "RequireValidUser")]
[ApiController]
[Route("[controller]")]
public class RisksController : ApiBaseController
{
    private readonly DALManager _dalManager;
    private IRiskManagementService _riskManagement;
    private ILogger _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserManagementService _userManagementService;
    
    public RisksController(
        ILogger logger, DALManager dalManager,
        IHttpContextAccessor httpContextAccessor,
        IUserManagementService userManagementService,
        IRiskManagementService riskManagement) : base(logger)
    {
        _logger = logger;
        _dalManager = dalManager;
        _riskManagement = riskManagement;
        _httpContextAccessor = httpContextAccessor;
        _userManagementService = userManagementService;
    }
    
    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Risk>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<Risk>> GetAll([FromQuery] string? status = null)
    {
        var userAccount =  UserHelper.GetUserName(_httpContextAccessor.HttpContext!.User.Identity);
        
        if (userAccount == null)
        {
            _logger.Error("Authenticated userAccount not found");
            throw new UserNotFoundException();
        }
        
        var user = _userManagementService.GetUser(userAccount);
        if (user == null )
        {
            _logger.Error("Authenticated user not found");
            throw new UserNotFoundException();
        }
        
        // We have the logged user... Now we can list the risks it has access to
        
        var risks = new List<Risk>();

        try
        {
            risks = _riskManagement.GetUserRisks(user, status);

            return Ok(risks);
        }
        catch (UserNotAuthorizedException ex)
        {
            _logger.Warning($"The user {user.Name} is not authorized to see risks");
            return this.Unauthorized();
        }
        
        
    }
    
}