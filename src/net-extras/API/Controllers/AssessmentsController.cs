using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerServices;

namespace API.Controllers;

[Authorize(Policy = "RequireAssessmentAccess")]
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
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Assessment))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public ActionResult<Assessment> GetAssessment(int id)
    {

        try
        {
            Logger.Debug("Searching assessment with id {id}", id);
            var assessment = _assessmentsService.Get(id);
            if (assessment == null)
            {
                Logger.Error("Assessment with id {id} not found", id);
                return NotFound("Assessment not found");
            }
            
            return assessment;

        }catch(Exception ex)
        {
            Logger.Error(ex, "Error finding assessment");
            return StatusCode(500, "Error finding assessment");
        }

    }
    
    [HttpPost]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Assessment))]
    [ProducesResponseType(StatusCodes.Status409Conflict, Type = typeof(string))]
    public ActionResult<Assessment> CreateAssessment([FromBody] Assessment assessment)
    {

        try
        {
            Logger.Debug("Creating new assessment");
            var operResult = _assessmentsService.Create(assessment);
            if (operResult.Item1 == 1)
            {
                return Conflict("Assessment already exists");
            }

            if (operResult.Item1 == 0)
            {
                return CreatedAtAction(nameof(GetAssessment), new { id = assessment.Id }, assessment);
            }
            
            return StatusCode(500, "Error creating assessment");
            

        }catch(Exception ex)
        {
            Logger.Error("Error creating assessment: {0}", ex.Message);
            return StatusCode(500, "Error creating assessment");
        }

    }
    
    
    [HttpGet]
    [Route("{id}/answers")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AssessmentAnswer>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public ActionResult<List<AssessmentAnswer>> ListAssessmentAnswers(int id)
    {

        try
        {
            Logger.Debug("Searching answers for assessment with id {id}", id);
            var assessmentAnswers = _assessmentsService.GetAnswers(id);
            if (assessmentAnswers == null)
            {
                Logger.Error("Answers for assessment with id {id} not found", id);
                return NotFound("Assessment not found");
            }
            
            return assessmentAnswers;

        }catch(Exception ex)
        {
            Logger.Error(ex, "Error finding assessment answers");
            return StatusCode(500, "Answers not found");
        }

    }
    
    [HttpGet]
    [Route("{id}/questions")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<AssessmentQuestion>))]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(string))]
    public ActionResult<List<AssessmentQuestion>> ListAssessmentQuestions(int id)
    {

        try
        {
            Logger.Debug("Searching questions for assessment with id {id}", id);
            var assessmentQuestions = _assessmentsService.GetQuestions(id);
            if (assessmentQuestions == null)
            {
                Logger.Error("Questions for assessment with id {id} not found", id);
                return NotFound("Questions not found");
            }
            
            return assessmentQuestions;

        }catch(Exception ex)
        {
            Logger.Error(ex, "Error finding assessment questions");
            return StatusCode(500, "Questions not found");
        }

    }
}