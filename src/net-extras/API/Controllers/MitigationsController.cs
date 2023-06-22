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
    private IMitigationManagementService _mitigationManagement;
    #endregion
    
    public MitigationsController(
        ILogger logger,
        IHttpContextAccessor httpContextAccessor,
        IUserManagementService userManagementService,
        IMitigationManagementService mitigationManagementService,
        IRiskManagementService riskManagement) : base(logger, httpContextAccessor, userManagementService)
    {
        _riskManagement = riskManagement;
        _mitigationManagement = mitigationManagementService;
    }
    
    #region METHODS
    /// <summary>
    /// Gets a Mitigation by it´s Id
    /// </summary>
    /// <param name="id">Mitigation Id</param>
    /// <returns></returns>
    [HttpGet]
    [Route("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<Risk>))]
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
    /// Gets a Mitigation by it´s Id
    /// </summary>
    /// <param name="id">Mitigation Id</param>
    /// <returns></returns>
    [HttpGet]
    [Authorize(Policy = "RequireValidUser")]
    [Route("strategies")]
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
    #endregion
}