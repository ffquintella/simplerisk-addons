using DAL.Entities;

namespace ServerServices;

public interface IRoleManagementService
{
    List<string> GetRolePermissions(int roleId);
    
    Role GetRole(int roleId);
}