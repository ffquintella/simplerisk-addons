using DAL.Entities;

namespace ServerServices.Interfaces;

public interface ITeamManagementService
{
    /// <summary>
    /// Get all teams
    /// </summary>
    /// <returns>List of teams</returns>
    public List<Team> GetAll();
}