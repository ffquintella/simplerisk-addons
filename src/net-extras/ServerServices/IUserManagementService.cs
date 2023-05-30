using System;
using System.Collections.Generic;
using DAL.Entities;
using Model.DTO;

namespace ServerServices;

public interface IUserManagementService
{
    User? GetUser(string userName);
    List<string> GetUserPermissions(int userId);

    String GetUserName(int id);
    
    List<UserListing> ListActiveUsers();


}