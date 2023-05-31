using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model.Exceptions;
using ServerServices;
using ILogger = Serilog.ILogger;

namespace API.Controllers;

[Authorize(Policy = "RequireAdminOnly")]
[ApiController]
[Route("[controller]")]
public class UsersController: ApiBaseController
{

    public UsersController(ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        IUserManagementService userManagementService) : base(logger, httpContextAccessor, userManagementService)
    {
    }
    
    
    [Authorize(Policy = "RequireValidUser")]
    [HttpGet]
    [Route("Name/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<string> GetUserName(int id)
    {
        
        try
        {
            var name  = _userManagementService.GetUserName(id);
            return Ok(name);
        }
        catch (DataNotFoundException ex)
        {
            Logger.Warning($"The user with id: {id} was not found: {ex.Message}");
            return NotFound($"The user with the id:{ex.Identification} was not found");
        }
        
    }
    
    //listings
    [Authorize(Policy = "RequireValidUser")]
    [HttpGet]
    [Route("Listings")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<UserListing>> ListUsers()
    {
        
        try
        {
            var users = _userManagementService.ListActiveUsers();
            return Ok(users);
        }
        catch (Exception ex)
        {
            Logger.Warning($"Error listing users: {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error listing user");
        }
        
    }
}