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

    private User GetUser()
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

        return user;
    }
    
    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Risk>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<Risk>> GetAll([FromQuery] string? status = null)
    {

        var user = GetUser();

        var risks = new List<Risk>();

        try
        {
            risks = _riskManagement.GetUserRisks(user, status);

            return Ok(risks);
        }
        catch (UserNotAuthorizedException ex)
        {
            _logger.Warning($"The user {user.Name} is not authorized to see risks message: {ex.Message}");
            return this.Unauthorized();
        }
        
        
    }

    [HttpGet]
    [Authorize(Policy = "RequireMgmtReviewAccess")]
    [Route("ManagementReviews")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Risk>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<MgmtReview>> GetAllMangementReviews([FromQuery] string? status = null)
    {
        var reviews = new List<MgmtReview>();


        return reviews;
    }

    [HttpGet]
    [Route("MyRisks")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Risk>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<Risk>> GetMyRisks([FromQuery] string? status = null)
    {
        var user = GetUser();

        var risks = new List<Risk>();

        try
        {
            risks = _riskManagement.GetUserRisks(user, status);

            return Ok(risks);
        }
        catch (UserNotAuthorizedException ex)
        {
            _logger.Warning($"The user {user.Name} is not authorized to see risks message: {ex.Message}");
            return this.Unauthorized();
        }
        

        return NotFound(risks);
    }


    [HttpGet]
    [Route("NeedingMgmtReviews")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Risk>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<Risk>> GetRisksNeedingMgmtReviews([FromQuery] string? status = null)
    {
        var user = GetUser();

        var risks = new List<Risk>();
        try
        {
            risks = _riskManagement.GetRisksNeedingReview(status);

            return Ok(risks);
        }
        catch (UserNotAuthorizedException ex)
        {
            _logger.Warning($"The user {user.Name} is not authorized to see risks message: {ex.Message}");
            return this.Unauthorized();
        }

    }
    
}