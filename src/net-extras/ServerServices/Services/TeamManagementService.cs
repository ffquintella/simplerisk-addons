using DAL;
using DAL.Entities;
using Model.Exceptions;
using Serilog;
using ServerServices.Interfaces;

namespace ServerServices.Services;

public class TeamManagementService: ITeamManagementService
{
    private DALManager _dalManager;
    private ILogger _log;


    public TeamManagementService(
        ILogger logger, 
        DALManager dalManager
    )
    {
        _dalManager = dalManager;
        _log = logger;
    }
    
    public List<Team> GetAll()
    {
        using var context = _dalManager.GetContext();
        var teams = context.Teams.ToList();
        if (teams == null)
        {
            Log.Error("Error Listing teams");
            throw new DataNotFoundException("Team", "No teams found");
        }

        return teams;
    }

    public List<Team> GetByMitigationId(int id)
    {
        using var context = _dalManager.GetContext();
        var teams = context.Teams
            .Where(t => context.MitigationToTeams.Where(m => m.MitigationId == id).Select(m => m.TeamId)
                .Contains(t.Value))
            .ToList();
            
        if (teams == null)
        {
            Log.Error("Error Listing teams");
            throw new DataNotFoundException("Team", "No teams found");
        }

        return teams;
    }
}