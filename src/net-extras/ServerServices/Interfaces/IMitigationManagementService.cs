using DAL.Entities;

namespace ServerServices.Interfaces;

public interface IMitigationManagementService
{
    
    /// <summary>
    /// Gets one mitigation by it´s ID
    /// </summary>
    /// <param name="id">Mitigaiton ID</param>
    /// <returns>Mitigation Object</returns>
    public Mitigation GetById(int id);

    /// <summary>
    /// Gets one mitigation by it´s RiskId
    /// </summary>
    /// <param name="id">Risk Id</param>
    /// <returns>Mitigation Object</returns>
    public Mitigation GetByRiskId(int id);
    
    /// <summary>
    /// List the mitigation strategies
    /// </summary>
    /// <returns></returns>
    public List<PlanningStrategy> ListStrategies();
    
    /// <summary>
    /// List the mitigation efforts
    /// </summary>
    /// <returns></returns>
    public List<MitigationEffort> ListEfforts();
}