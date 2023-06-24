using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Exceptions;
using ServerServices.Interfaces;
using ILogger = Serilog.ILogger;

namespace API.Controllers;

[Authorize(Policy = "RequireMitigation")]
[ApiController]
[Route("[controller]")]
public class MitigationsController: ApiBaseController
{
    #region FIELDS
    private IRiskManagementService _riskManagement;
    private readonly IMitigationManagementService _mitigationManagement;
    private ITeamManagementService _teamManagement;
    #endregion
    
    public MitigationsController(
        ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        IUserManagementService userManagementService,
        IMitigationManagementService mitigationManagementService,
        ITeamManagementService teamManagementService,
        IRiskManagementService riskManagement) : base(logger, httpContextAccessor, userManagementService)
    {
        _riskManagement = riskManagement;
        _mitigationManagement = mitigationManagementService;
        _teamManagement = teamManagementService;
    }
    
    #region METHODS
    /// <summary>
    /// Gets a Mitigation by it´s Id
    /// </summary>
    /// <param name="id">Mitigation Id</param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Mitigation))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<Mitigation> GetById(int id)
    {

        var user = GetUser();

        Logger.Information("User:{UserValue} got mitigation with id={Id}", user.Value, id);

        Mitigation mitigation;
        
        try
        {
            mitigation = _mitigationManagement.GetById(id);
        }
        catch (DataNotFoundException ex)
        {
            Logger.Warning("The mitigation with id: {Id} does not exists: {ExMessage}", id, ex.Message);
            return this.NotFound();
        }
        catch (Exception ex)
        {
            Logger.Error("Internal error getting mitigation: {Message}", ex.Message);
            return StatusCode(500);
        }

        return Ok(mitigation);

    }
    
    /// <summary>
    /// Gets mitigation teams by mitigation Ids
    /// </summary>
    /// <param name="id">Mitigation Id</param>
    /// <returns>List of Mitigation Teams</returns>
    [HttpGet]
    [Route("{id}/Teams")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Team>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<Team>> GetTeamsById(int id)
    {

        var user = GetUser();

        Logger.Information("User:{UserValue} got mitigation´s {Id} teams", user.Value, id);

        List<Team> teams;
        
        try
        {
            teams = _teamManagement.GetByMitigationId(id);
        }
        catch (DataNotFoundException ex)
        {
            Logger.Warning("There is no teams with mitigation: {Id} : {ExMessage}", id, ex.Message);
            return this.NotFound();
        }
        catch (Exception ex)
        {
            Logger.Error("Internal error getting mitigation´s teams: {Message}", ex.Message);
            return StatusCode(500);
        }

        return Ok(teams);

    }
    
    /// <summary>
    /// List mitigation strategies
    /// </summary>
    /// <returns>List of mitigation strategies </returns>
    [HttpGet]
    [Authorize(Policy = "RequireValidUser")]
    [Route("Strategies")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<PlanningStrategy>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<PlanningStrategy>> ListMitigationStrategies()
    {

        var user = GetUser();

        Logger.Information("User:{UserValue} listed mitigation strategies", user.Value);

        List<PlanningStrategy> strategies;
        
        try
        {
            strategies = _mitigationManagement.ListStrategies();
        }
        catch (Exception ex)
        {
            Logger.Error("Internal error getting strategies: {Message}", ex.Message);
            return StatusCode(500);
        }

        return Ok(strategies);

    }
    
    /// <summary>
    /// List mitigation efforts
    /// </summary>
    /// <returns>List of mitigation efforts</returns>
    [HttpGet]
    [Authorize(Policy = "RequireValidUser")]
    [Route("Efforts")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MitigationEffort>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<MitigationEffort>> ListMitigationEffort()
    {

        var user = GetUser();

        Logger.Information("User:{UserValue} listed mitigation efforts", user.Value);

        List<MitigationEffort> efforts;
        
        try
        {
            efforts = _mitigationManagement.ListEfforts();
        }
        catch (Exception ex)
        {
            Logger.Error("Internal error getting strategies: {Message}", ex.Message);
            return StatusCode(500);
        }

        return Ok(efforts);

    }
    
    /// <summary>
    /// Lists Mitigation costs
    /// </summary>
    /// <returns>List of mitigation costs</returns>
    [HttpGet]
    [Authorize(Policy = "RequireValidUser")]
    [Route("Costs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<MitigationCost>))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<List<MitigationCost>> ListMitigationCosts()
    {

        var user = GetUser();

        Logger.Information("User:{UserValue} listed mitigation costs", user.Value);

        List<MitigationCost> costs;
        
        try
        {
            costs = _mitigationManagement.ListCosts();
        }
        catch (Exception ex)
        {
            Logger.Error("Internal error getting costs: {Message}", ex.Message);
            return StatusCode(500);
        }

        return Ok(costs);

    }
    #endregion
}