using Model.DTO;

namespace ClientServices.Interfaces;

public interface IUsersService
{
    string GetUserName(int id);

    List<UserListing> ListUsers();

}