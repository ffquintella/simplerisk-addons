using System.Collections.Generic;
using DAL.Entities;

namespace ServerServices;

public interface IPermissionManagementService
{
    bool UserHasPermission(User user, string Permission);
    List<string> GetUserPermissions(User user);
}