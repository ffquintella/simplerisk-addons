using DAL;
using DAL.Context;
using DAL.Entities;
using Microsoft.Extensions.Logging;

namespace ServerServices;

public class RoleManagementService: IRoleManagementService
{
    private SRDbContext? _dbContext = null;
    private ILogger _log;

    public RoleManagementService(DALManager dalManager,
        ILoggerFactory logger )
    {
        _dbContext = dalManager.GetContext();
        _log = logger.CreateLogger(nameof(UserManagementService));
    }


    public List<string> GetRolePermissions(int roleId)
    {
        var roles = _dbContext!.RoleResponsibilities.Where(rlr => rlr.RoleId == roleId);

        var permissions = _dbContext!.Permissions.Where(p => roles.Any(r => r.PermissionId == p.Id));

        var result = new List<string>();

        foreach (var permission in permissions)
        {
            result.Add(permission.Key);
        }

        return result;
    }

    public Role? GetRole(int roleId)
    {
        var role = _dbContext!.Roles.Find(roleId);
        return role;
    }
}