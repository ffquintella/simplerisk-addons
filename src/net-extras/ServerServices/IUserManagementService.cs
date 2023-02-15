using DAL.Entities;

namespace ServerServices;

public interface IUserManagementService
{
    User? GetUser(string userName);
    List<string> GetUserPermissions(int userId);

    String GetUserName(int id);


}