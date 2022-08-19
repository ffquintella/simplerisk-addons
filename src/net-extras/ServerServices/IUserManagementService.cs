using DAL.Entities;

namespace ServerServices;

public interface IUserManagementService
{
    User? GetUser(string userName);
}