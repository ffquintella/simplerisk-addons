using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Registration;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class Registration : ControllerBase
{
    [AllowAnonymous]
    [HttpPost]
    [Route("/")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status412PreconditionFailed, Type = typeof(string))]
    public ActionResult<string> Register([FromBody] RegistrationRequest request)
    {
        string hashCode = String.Format("{0:X}", request.Id.GetHashCode());
        return Ok(hashCode);
        return this.StatusCode(StatusCodes.Status412PreconditionFailed, "Already exists");
    }
}