using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.DTO;
using Model.Exceptions;
using Model.Users;
using ServerServices;
using ServerServices.Interfaces;
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
    
    [Authorize(Policy = "RequireValidUser")]
    [HttpGet]
    [Route("{id}/ChangePassword")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<string> ChangePassword(int id, [FromBody] ChangePasswordRequest? changePasswordRequest)
    {
        if (changePasswordRequest == null)
            return BadRequest("The request is empty");
        
        var loggedUser = GetUser();

        if (!loggedUser.Admin)
        {
            // Then the user can only change it´s own password
            if (loggedUser.Value != id)
            {
                Logger.Warning($"The user with id: {id} was not found: {loggedUser.Value} tried to change password of {id}");
                return Unauthorized($"The user with the id:{loggedUser.Value} is not authorized to change the password of {id}");
            }
            
            // Now let´s verify if the old password is correct
            
            
        }
        
        
        
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