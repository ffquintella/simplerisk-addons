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

        if (user.Admin) return GetAll();
        
        // If the user not an admin we will check if the user has permission to modify risks  if so he can read all 
        if (UserHasRisksPermission(user, "modify_risks")) return GetAll();
        
        // if not he can only see the risks associated to himself or that he created
        using (var context = _dalManager.GetContext())
        {
            risks = context.Risks.Where(r => r.Owner == user.Value
                                                 || r.SubmittedBy == user.Value
                                                 || r.Manager == user.Value).ToList();
            
        }

        return risks;
    }

    public List<Risk> GetAll()
    {
        var risks = new List<Risk>();

        using (var contex = _dalManager.GetContext())
        {
            risks = contex.Risks.ToList();
        }
        
        return risks;
    }

    private bool UserHasRisksPermission(User user, string permission = "riskmanagement")
    {
        if (user.Admin) return true;

        var permissions = _roleManagement.GetRolePermissions(user.RoleId);

        if (permissions.Contains(permission)) return true;
        
        return false;
    }
}