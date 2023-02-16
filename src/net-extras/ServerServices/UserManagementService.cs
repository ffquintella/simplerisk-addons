using System;
using System.Collections.Generic;
using System.Text;
using DAL;
using DAL.Context;
using DAL.Entities;
using Microsoft.Extensions.Logging;
using Model.Exceptions;
using System.Linq;

namespace ServerServices;

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