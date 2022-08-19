using System.Text;
using DAL;
using DAL.Context;
using DAL.Entities;
using Microsoft.Extensions.Logging;

namespace ServerServices;

public class UserManagementService: IUserManagementService
{
    private SRDbContext? _dbContext = null;
    private ILogger _log;

    public UserManagementService(DALManager dalManager,
        ILoggerFactory logger )
    {
        _dbContext = dalManager.GetContext();
        _log = logger.CreateLogger(nameof(UserManagementService));
    }

    public User? GetUser(string userName)
    {
        var user = _dbContext?.Users?
            .Where(u => u.Username == Encoding.UTF8.GetBytes(userName))
            .FirstOrDefault();

        return user;
    }
}