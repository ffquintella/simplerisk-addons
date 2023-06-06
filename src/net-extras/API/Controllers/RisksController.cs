using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using API.Tools;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Model.Exceptions;
using ServerServices;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using dk.nita.saml20;
using ILogger = Serilog.ILogger;

namespace API.Controllers;

[Authorize(Policy = "RequireRiskmanagement")]
[ApiController]
[Route("[controller]")]
public class RisksController : ApiBaseController
{

    private IRiskManagementService _riskManagement;

    public RisksController(
        ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        IUserManagementService userManagementService,
        IRiskManagementService riskManagement) : base(logger, httpContextAccessor, userManagementService)
    {
        _riskManagement = riskManagement;
    }
    
    
    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Risk>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<Risk>> GetAll([FromQuery] string? status = null)
    {

        var user = GetUser();

        Logger.Information($"User:{user.Value} listed all risks");
        
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
    
    // Create new Risk
    [HttpPost]
    [Route("")]
    [Authorize(Policy = "RequireSubmitRisk")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(Risk))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<Risk> Create([FromBody] Risk? risk = null)
    {

        var user = GetUser();

        Logger.Information($"User:{user.Value} submited a risk");

        try
        {
            risk.SubmittedBy = user.Value;
            risk.Status = "New";
            
            var crisk = _riskManagement.CreateRisk(risk);

            if (crisk != null) return Created("risks/" + crisk.Id, crisk);
            
            return StatusCode(StatusCodes.Status500InternalServerError);
        }
        catch (UserNotAuthorizedException ex)
        {
            Logger.Warning($"The user {user.Name} is not authorized to create risks message: {ex.Message}");
            return this.Unauthorized();
        }
        
        
    }
    
    // Check if risk subejct exists new Risk
    [HttpGet]
    [Route("Exists")]
    [Authorize(Policy = "RequireSubmitRisk")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<bool> Create([FromQuery] string? subject = null)
    {

        try
        {
            if (subject != null)
            {
                var exists = _riskManagement.SubjectExists(subject);
                if (exists) return StatusCode(StatusCodes.Status200OK);
                return StatusCode(StatusCodes.Status404NotFound);
            }
            

            return StatusCode(StatusCodes.Status500InternalServerError);
            
        }
        catch (UserNotAuthorizedException ex)
        {
            var user = GetUser();
            Logger.Warning($"The user {user.Value} is not authorized to check risks subjects message: {ex.Message}");
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
        
        var user = GetUser();

        Logger.Information($"User:{user.Value} listed all management reviews");
        
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

        Logger.Information($"User:{user.Value} listed own risks");
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
    [Route("Categories/{id}")]
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
    [Route("Categories")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Risk>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<Category>> GetRiskCategories()
    {
        
        try
        {
            var cats  = _riskManagement.GetRiskCategories();
            return Ok(cats);
            
        }
        catch (DataNotFoundException ex)
        {
            Logger.Warning($"Error Listing categories {ex.Message}");
            return StatusCode(StatusCodes.Status500InternalServerError);

        }
        
    }
    
    [HttpGet]
    [Route("Catalogs/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Risk>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<RiskCatalog> GetRiskCatalog(int id)
    {
        
        try
        {
            var cat  = _riskManagement.GetRiskCatalog(id);
            return Ok(cat);
            
        }
        catch (DataNotFoundException ex)
        {
            Logger.Warning($"The catalog {id} was not found: {ex.Message}");
            return NotFound();

        }
        
    }
    
    [HttpGet]
    [Route("Catalogs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Risk>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<RiskCatalog>> GetRisksCatalog([FromQuery] string list = "")
    {
        var all = false;
        if (list == "") all = true;
        
        var regex = @"^\d+(,\d+)*$";
        var match = Regex.Match(list, regex, RegexOptions.IgnoreCase);

        if (all == false && !match.Success)
        {
            Logger.Warning($"Invalid catalog list format");
            return StatusCode(409);
        }
        
        try
        {
            if (all)
            {
                var cats  = _riskManagement.GetRiskCatalogs();
                return Ok(cats);
            }
            
            var sids = list.Split(',').ToList();

            var ids = new List<int>();

            foreach (var sid in sids)
            {
                ids.Add(Int32.Parse(sid));
            }
            
            var cat  = _riskManagement.GetRiskCatalogs(ids);
            return Ok(cat);
            
        }
        catch (DataNotFoundException ex)
        {
            Logger.Warning($"The catalogs {list} was not found: {ex.Message}");
            return NotFound();

        }
        
    }
    
    [HttpGet]
    [Route("Sources/{id}")]
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
    [Route("Sources")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Risk>))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<Source> GetRiskSources()
    {
        
        try
        {
            var srcs  = _riskManagement.GetRiskSources();
            return Ok(srcs);
            
        }
        catch (DataNotFoundException ex)
        {
            Logger.Warning($"Erro listing the sources: {ex.Message}");
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