using System.Text;
using DAL;
using DAL.Entities;
using Microsoft.Extensions.Logging;
using Model.Exceptions;
using Model.DTO;
using ServerServices.Interfaces;
using static BCrypt.Net.BCrypt;
using static Tools.Extensions.StringExt;

namespace ServerServices.Services;

public class UserManagementService: IUserManagementService
{
    //private SRDbContext? _dbContext = null;
    private DALManager? _dalManager;
    private ILogger _log;
    private IRoleManagementService _roleManagementService;
    private readonly IPermissionManagementService _permissionManagement;

    public UserManagementService(DALManager dalManager,
        ILoggerFactory logger,
        IRoleManagementService roleManagementService,
        IPermissionManagementService permissionManagementService)
    {
        //_dbContext = dalManager.GetContext();
        _dalManager = dalManager;
        _log = logger.CreateLogger(nameof(UserManagementService));
        _roleManagementService = roleManagementService;
        _permissionManagement = permissionManagementService;
    }

    public User? GetUser(string userName)
    {
        var dbContext = _dalManager!.GetContext();
        var user = dbContext?.Users?
            .Where(u => u.Username == Encoding.UTF8.GetBytes(userName))
            .FirstOrDefault();

        return user;
    }

    public bool VerifyPassword(string username, string password)
    {
        return VerifyPassword(GetUser(username), password);
    }
    public bool VerifyPassword(int userId, string password)
    {
        return VerifyPassword(GetUserById(userId), password);
    }

    public bool VerifyPassword(User? user, string password)
    {
        if (user.Type == "saml")
        {
            throw new Exception("Cannot verify password of saml users");
        }
        
        if (user == null) return false;
        
        if(user.Lockout == 1)
        {
            return false;
        }
        
        var valid = Verify(password, Encoding.UTF8.GetString(user.Password));
        
        return valid; 
    }

    public bool ChangePassword(int userId, string password)
    {
        var dbContext = _dalManager!.GetContext();

        var user = GetUserById(userId);

        if (user == null) return false;
        if (user.Type == "saml")
        {
            throw new Exception("Cannot change password of saml users");
        } 

        
        try
        {
            //var salt = GenerateSalt();
            //user.Salt = salt.Replace("$2a$", "").Truncate(20, "");
            user.Password = Encoding.UTF8.GetBytes(HashPassword(password, 15));
            dbContext?.Users?.Update(user);
            dbContext?.SaveChanges();
            _log.LogWarning("Password changed for user {userId}", userId);
            return true;
        }
        catch (Exception ex)
        {
            _log.LogError(ex, "Error changing password for user {userId}", userId);
        }

        
        return false;
    }
    public User? GetUserById(int userId)
    {
        var dbContext = _dalManager!.GetContext();
        var user = dbContext?.Users?
            .Where(u => u.Value == userId)
            .FirstOrDefault();
      
        
        return user;
    }

    public String GetUserName(int id)
    {
        var user = GetUserById(id);
        if (user == null)
        {
            throw new DataNotFoundException("user", id.ToString());
        }
        return user.Name;
    }

    // List all active users
    public List<UserListing> ListActiveUsers()
    {
        var list = new List<UserListing>();
        
        var dbContext = _dalManager!.GetContext();
        var users = dbContext?.Users?
            .Where(u => u.Enabled == true)
            .ToArray();
        if (users == null) return list;
        
        foreach (var user in users)
        {
            var ul = new UserListing
            {
                Id = user.Value,
                Name = user.Name
            };
            list.Add(ul);
        }
        
        return list;
    }
    
    public List<string> GetUserPermissions(int userId)
    {
        var user = GetUserById(userId);
        if (user == null)
        {
            throw new UserNotFoundException();
        }

        return _permissionManagement.GetUserPermissions(user);
    }


}