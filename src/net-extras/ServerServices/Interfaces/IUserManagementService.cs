using System;
using System.Collections.Generic;
using DAL.Entities;
using Model.DTO;

namespace ServerServices.Interfaces;

public interface IUserManagementService
{
    User? GetUser(string userName);
    List<string> GetUserPermissions(int userId);

    String GetUserName(int id);
    
    List<UserListing> ListActiveUsers();
    
    bool VerifyPassword(string username, string password);
    bool VerifyPassword(int userId, string password);
    
    bool VerifyPassword(User? user, string password);
    
    bool ChangePassword(int userId, string password);

    public User? GetUserById(int userId);

}