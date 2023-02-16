using System.Collections.Generic;
using DAL;
using DAL.Entities;
using Model.Exceptions;
using Serilog;
using System.Linq;

namespace ServerServices;

public class RiskManagementService: IRiskManagementService
{
    private DALManager _dalManager;
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

    /// <summary>
    /// Gets the risks associated to a user
    /// </summary>
    /// <param name="user"></param>
    /// <param name="status"></param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    /// <exception cref="UserNotAuthorizedException"></exception>
    public List<Risk> GetUserRisks(User user, string? status = null)
    {
        if (user == null) throw new InvalidParameterException("user","User cannot be null");
        
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

    public Category GetRiskCategory(int id)
    {
        using (var contex = _dalManager.GetContext())
        {
            
            var cat = contex.Categories.Where(c => c.Value == id).FirstOrDefault();

            if (cat == null)
            {
                throw new DataNotFoundException("Category", id.ToString());
            }

            return cat;
        }
    }

    public RiskCatalog GetRiskCatalog(int id)
    {
        using (var contex = _dalManager.GetContext())
        {
            
            var cat = contex.RiskCatalogs.Where(c => c.Id == id).FirstOrDefault();

            if (cat == null)
            {
                throw new DataNotFoundException("Catalog", id.ToString());
            }

            return cat;
        }
    }
    
    public List<RiskCatalog> GetRiskCatalogs(List<int> ids)
    {
        using (var contex = _dalManager.GetContext())
        {

            var cats = contex.RiskCatalogs.Where(c => ids.Contains(c.Id)).ToList();

            if (cats == null)
            {
                string sids = "";
                foreach (var id in ids)
                {
                    sids += id + ",";
                }
                throw new DataNotFoundException("Catalog", sids);
            }

            return cats;
        }
    }
    
    public Source GetRiskSource(int id)
    {
        using (var contex = _dalManager.GetContext())
        {
            
            var src = contex.Sources.Where(c => c.Value == id).FirstOrDefault();

            if (src == null)
            {
                throw new DataNotFoundException("Source", id.ToString());
            }

            return src;
        }
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