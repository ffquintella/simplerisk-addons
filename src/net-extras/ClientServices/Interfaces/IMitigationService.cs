﻿using DAL.Entities;

namespace ClientServices.Interfaces;

public interface IMitigationService
{
    /// <summary>
    /// Gets mitigation by Risk Id
    /// </summary>
    /// <param name="id">Risk Id</param>
    /// <returns></returns>
    public Mitigation? GetByRiskId(int id);
    
    /// <summary>
    /// Gets mitigation by id
    /// </summary>
    /// <param name="id">Mitigation Id</param>
    /// <returns></returns>
    public Mitigation? GetById(int id);
    
    /// <summary>
    /// Gets team by mitigation id
    /// </summary>
    /// <param name="id">Mitigation Id</param>
    /// <returns>Team</returns>
    public List<Team>? GetTeamsById(int id);
    
    /// <summary>
    /// Gets the mitigations strategies avaliable
    /// </summary>
    /// <returns>List of PlaningStrategy</returns>
    public List<PlanningStrategy>? GetStrategies();
    
    /// <summary>
    /// Gets the possible list of mitigation costs
    /// </summary>
    /// <returns></returns>
    public List<MitigationCost>? GetCosts();
    
    
    /// <summary>
    /// Gets the possible list of mitigation efforts
    /// </summary>
    /// <returns></returns>
    public List<MitigationEffort>? GetEfforts();
    
    /// <summary>
    /// Creates a new mitigation
    /// </summary>
    /// <param name="mitigation"></param>
    /// <returns>Mitigation object</returns>
    public Mitigation? Create(Mitigation mitigation);
    
    /// <summary>
    /// Deletes all teams associations for a mitigation
    /// </summary>
    /// <param name="mitigationId"></param>
    public void DeleteTeamsAssociations(int mitigationId);
    
    
    /// <summary>
    /// Associates a mitigation to a team
    /// </summary>
    /// <param name="mitigationId"></param>
    /// <param name="teamId"></param>
    public void AssociateMitigationToTeam(int mitigationId, int teamId);
}