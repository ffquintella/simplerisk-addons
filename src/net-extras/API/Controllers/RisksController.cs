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

    private IRiskManagementService _riskManagement;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserManagementService _userManagementService;
    
    public RisksController(
        ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        IUserManagementService userManagementService,
        IRiskManagementService riskManagement) : base(logger)
    {
        _riskManagement = riskManagement;
        _httpContextAccessor = httpContextAccessor;
        _userManagementService = userManagementService;
    }

    private User GetUser()
    {
        var userAccount =  UserHelper.GetUserName(_httpContextAccessor.HttpContext!.User.Identity);
        
        if (userAccount == null)
        {
            Logger.Error("Authenticated userAccount not found");
            throw new UserNotFoundException();
        }
        
        var user = _userManagementService.GetUser(userAccount);
        if (user == null )
        {
            Logger.Error("Authenticated user not found");
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
            Logger.Warning($"The user {user.Name} is not authorized to see risks message: {ex.Message}");
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

            if(risks.Count > 0) return Ok(risks);
            return NotFound(risks);
        }
        catch (UserNotAuthorizedException ex)
        {
            Logger.Warning($"The user {user.Name} is not authorized to see risks message: {ex.Message}");
            return this.Unauthorized();
        }
        
    }

    [HttpGet]
    [Route("Category/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Risk>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<Category> GetRiskCategory(int id)
    {
        
        try
        {
            var cat  = _riskManagement.GetRiskCategory(id);
            return Ok(cat);
            
        }
        catch (DataNotFoundException ex)
        {
            Logger.Warning($"The category {id} was not found: {ex.Message}");
            return NotFound();

        }
        
    }
    
    
    [HttpGet]
    [Route("Source/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Risk>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<Source> GetRiskSource(int id)
    {
        
        try
        {
            var src  = _riskManagement.GetRiskSource(id);
            return Ok(src);
            
        }
        catch (DataNotFoundException ex)
        {
            Logger.Warning($"The category {id} was not found: {ex.Message}");
            return NotFound();

        }
        
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
            Logger.Warning($"The user {user.Name} is not authorized to see risks message: {ex.Message}");
            return this.Unauthorized();
        }

    }
    
}