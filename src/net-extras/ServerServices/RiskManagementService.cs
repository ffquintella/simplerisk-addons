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

    public List<Risk> GetUserRisks(User user, string? status = null)
    {
        if (!UserHasRisksPermission(user)) throw new UserNotAuthorizedException(user.Name, user.Value, "risks");
        var risks = new List<Risk>();

        if (user.Admin) return GetAll(status);
        
        // If the user not an admin we will check if the user has permission to modify risks  if so he can read all 
        if (UserHasRisksPermission(user, "modify_risks")) return GetAll();
        
        // if not he can only see the risks associated to himself or that he created
        using (var context = _dalManager.GetContext())
        {
            if (status != null)
            {
                risks = context.Risks.Where(r => r.Status == status && (r.Owner == user.Value
                                                 || r.SubmittedBy == user.Value
                                                 || r.Manager == user.Value)).ToList();
            }
            else
            {
                risks = context.Risks.Where(r => r.Owner == user.Value
                                                 || r.SubmittedBy == user.Value
                                                 || r.Manager == user.Value).ToList();
            }


            
        }

        return risks;
    }

    public List<Risk> GetAll(string? status = null)
    {
        var risks = new List<Risk>();

        using (var contex = _dalManager.GetContext())
        {
            
            if (status != null)
            {
                risks = contex.Risks.Where(r => r.Status == status).ToList();
                
            } else risks = contex.Risks.ToList();
            
        }
        
        return risks;
    }

    public List<Risk> GetRisksNeedingReview(string? status = null)
    {
        var risks = new List<Risk>();

        using (var contex = _dalManager.GetContext())
        {
            if (status != null)
            {
                risks = contex.Risks.Where(r => r.Status == status)
                    .Where(r => !contex.MgmtReviews
                        .Select(mr => mr.RiskId)
                        .Contains(r.Id)
                    ).ToList();
                
            } else risks = contex.Risks
                .Where(r => !contex.MgmtReviews
                    .Select(mr => mr.RiskId)
                    .Contains(r.Id)
                ).ToList();
            
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