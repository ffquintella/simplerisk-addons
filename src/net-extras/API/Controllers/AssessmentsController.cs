using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerServices;

namespace API.Controllers;

[Authorize(Policy = "RequireAdminOnly", Roles = "Administrator")]
[ApiController]
[Route("[controller]")]
public class AssessmentsController : ApiBaseController
{

    private IAssessmentsService _assessmentsService;
    
    public AssessmentsController(Serilog.ILogger logger, IAssessmentsService assessmentsService) : base(logger)
    {
        _assessmentsService = assessmentsService;
    }
  
    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Assessment>))]
    public ActionResult<List<Assessment>> GetAll()
    {

        try
        {
            var assessments = _assessmentsService.List();
            Logger.Debug("Listing all assessments");
            return assessments;

        }catch(Exception ex)
        {
            Logger.Error(ex, "Error listing all assessments");
            return StatusCode(500, "Error listing all assessments");
        }

    }
    
}