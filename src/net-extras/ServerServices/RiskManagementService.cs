using DAL;
using DAL.Entities;
using Model.Exceptions;
using Serilog;

namespace ServerServices;

public class RiskManagementService: IRiskManagementService
{
    private DALManager? _dalManager;
    private ILogger _log;
    private readonly IRoleManagementService _roleManagement;

    public RiskManagementService(
        ILogger logger, 
        DALManager dalManager,
        IRoleManagementService roleManagementService
        )
    {
        _dalManager = dalManager;
        _log = logger;
        _roleManagement = roleManagementService;
    }

    public List<Risk> GetUserRisks(User user)
    {
        if (!UserHasRisksPermission(user)) throw new UserNotAuthorizedException(user.Name, user.Value, "risks");
        var risks = new List<Risk>();


        return risks;
    }

    private bool UserHasRisksPermission(User user)
    {
        if (user.Admin) return true;

        var permissions = _roleManagement.GetRolePermissions(user.RoleId);

        if (permissions.Contains("Risks")) return true;
        
        return false;
    }
}