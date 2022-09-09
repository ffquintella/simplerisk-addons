using System.Text;
using DAL;
using DAL.Context;
using DAL.Entities;
using Microsoft.Extensions.Logging;
using Model.Exceptions;

namespace ServerServices;

public class UserManagementService: IUserManagementService
{
    //private SRDbContext? _dbContext = null;
    private DALManager? _dalManager;
    private ILogger _log;
    private IRoleManagementService _roleManagementService;

    public UserManagementService(DALManager dalManager,
        ILoggerFactory logger,
        IRoleManagementService roleManagementService)
    {
        //_dbContext = dalManager.GetContext();
        _dalManager = dalManager;
        _log = logger.CreateLogger(nameof(UserManagementService));
        _roleManagementService = roleManagementService;
    }

    public User? GetUser(string userName)
    {
        var dbContext = _dalManager!.GetContext();
        var user = dbContext?.Users?
            .Where(u => u.Username == Encoding.UTF8.GetBytes(userName))
            .FirstOrDefault();

        return user;
    }
    
    public User? GetUserById(int userId)
    {
        var dbContext = _dalManager!.GetContext();
        var user = dbContext?.Users?
            .Where(u => u.Value == userId)
            .FirstOrDefault();

        return user;
    }

    public List<string> GetUserPermissions(int userId)
    {
        var user = GetUserById(userId);
        if (user == null)
        {
            throw new UserNotFoundException();
        }
            
        var permissions = new List<string>();

        if (user.RoleId > 0)
        {
            var rolePermissions = _roleManagementService.GetRolePermissions(user.RoleId);
            permissions = rolePermissions;
        }
        
        var dbContext = _dalManager!.GetContext();
        
        var userPermissionsCon = dbContext!.PermissionToUsers.Where(pu => pu.UserId == userId).ToList();
        
        var userPermissions = dbContext!.Permissions.Where(p => userPermissionsCon.Select(upc=> upc.PermissionId ).Contains(p.Id)).ToList();

        var strUserPermissions = userPermissions.Select(up=>up.Key).ToList();

        var finalPermissions = permissions.Concat(strUserPermissions).ToList();
        
        return finalPermissions;
    }
    
    
}