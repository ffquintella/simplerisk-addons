using AutoMapper;
using DAL;
using DAL.Entities;
using Model.Exceptions;
using Serilog;
using ServerServices.Interfaces;

namespace ServerServices.Services;

public class RiskManagementService: IRiskManagementService
{
    private DALManager _dalManager;
    private ILogger _log;
    private readonly IRoleManagementService _roleManagement;
    private IMapper _mapper;

    public RiskManagementService(
        ILogger logger, 
        DALManager dalManager,
        IMapper mapper,
        IRoleManagementService roleManagementService
        )
    {
        _dalManager = dalManager;
        _log = logger;
        _roleManagement = roleManagementService;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets the risks associated to a user
    /// </summary>
    /// <param name="user">The user object</param>
    /// <param name="status">String representing the risk status</param>
    /// <param name="notStatus">String representing the status the risks should not have</param>
    /// <returns></returns>
    /// <exception cref="InvalidParameterException"></exception>
    /// <exception cref="UserNotAuthorizedException"></exception>
    public List<Risk> GetUserRisks(User user, string? status = null, string? notStatus = "Closed")
    {
        if (user == null) throw new InvalidParameterException("user","User cannot be null");
        
        if (!UserHasRisksPermission(user)) throw new UserNotAuthorizedException(user.Name, user.Value, "risks");
        
        //var risks = new List<Risk>();

        List<Risk> risks;

        if (user.Admin) return GetAll(status);
        
        // If the user not an admin we will check if the user has permission to modify risks  if so he can read all 
        if (UserHasRisksPermission(user, "modify_risks")) return GetAll();
        
        // if not he can only see the risks associated to himself or that he created
        using (var context = _dalManager.GetContext())
        {
            
            if (status != null && notStatus != null)
            {
                risks = context.Risks.Where(r => r.Status == status && r.Status != notStatus
                                                                    && (r.Owner == user.Value 
                                                                        || r.SubmittedBy == user.Value
                                                                        || r.Manager == user.Value)).ToList();
            }
            else if (status != null)
            {
                risks = context.Risks.Where(r => r.Status == status && (r.Owner == user.Value 
                                                                        || r.SubmittedBy == user.Value
                                                                        || r.Manager == user.Value)).ToList();
            }
            else if (notStatus != null)
            {
                risks = context.Risks.Where(r =>  r.Status != notStatus
                                                                    && (r.Owner == user.Value 
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

    public List<Risk> GetAll(string? status = null, string? notStatus = "Closed")
    {
        List<Risk> risks;
        //new List<Risk>();

        using (var context = _dalManager.GetContext())
        {
            if (status != null && notStatus != null)
            {
                risks = context.Risks.Where(r => r.Status == status && r.Status != notStatus).ToList();
            }
            else if (status != null)
            {
                risks = context.Risks.Where(r => r.Status == status).ToList();
            }
            else if (notStatus != null)
            {
                risks = context.Risks.Where(r => r.Status != notStatus).ToList();
            }
            else
            {
                risks = context.Risks.ToList();
            }
            
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
    
    public List<Category> GetRiskCategories()
    {
        using (var contex = _dalManager.GetContext())
        {
            
            var cats = contex.Categories.ToList();

            if (cats == null)
            {
                throw new DataNotFoundException("Categories", "");
            }

            return cats;
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

    public List<RiskCatalog> GetRiskCatalogs()
    {
        using (var contex = _dalManager.GetContext())
        {

            var cats = contex.RiskCatalogs.ToList();

            if (cats == null)
            {
                throw new DataNotFoundException("Catalog", "");
            }

            return cats;
        }
    }

    public bool SubjectExists(string subject)
    {
        using (var contex = _dalManager.GetContext())
        {
            var results = contex.Risks.Where(rsk => rsk.Subject == subject).Count();
            if (results > 0) return true;
            else return false;
        }
    }
    public Risk? CreateRisk(Risk risk)
    {
        using (var contex = _dalManager.GetContext())
        {
            risk.Id = 0;
            risk.SubmissionDate = DateTime.Now;
            risk.LastUpdate = DateTime.Now;
            contex.Risks.Add(risk);
            contex.SaveChanges();
            return risk;
        }
    }

    /// <summary>
    /// Saves a existing risk to the database
    /// </summary>
    /// <param name="risk">The risk to be saved (updated)</param>
    public void SaveRisk(Risk risk)
    {
        using (var context = _dalManager.GetContext())
        {
            var dbRisk = context.Risks.FirstOrDefault(r => r.Id == risk.Id);
            if (dbRisk == null) throw new Exception($"Unable to find risk with id:{risk.Id}");
            dbRisk = _mapper.Map(risk, dbRisk);
            context.SaveChanges();
        }
    }

    public void DeleteRisk(int id)
    {
        using (var context = _dalManager.GetContext())
        {
            var dbRisk = context.Risks.FirstOrDefault(r => r.Id == id);
            if (dbRisk == null) throw new DataNotFoundException("simplerisk",$"Unable to find risk with id:{id}");
            context.Risks.Remove(dbRisk);
            context.SaveChanges();
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
    
    public List<Source> GetRiskSources()
    {
        using (var contex = _dalManager.GetContext())
        {
            
            var src = contex.Sources.ToList();

            if (src == null)
            {
                throw new DataNotFoundException("Source" , "sources is empty");
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