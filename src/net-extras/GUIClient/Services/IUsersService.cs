using System.Collections.Generic;
using Model.DTO;

namespace GUIClient.Services;

public interface IUsersService
{
    string GetUserName(int id);

    List<UserListing> ListUsers();

}