using DAL.Entities;

namespace ServerServices;

public interface IRiskManagementService
{
    /// <summary>
    /// Lists all the risks the user has access to
    /// </summary>
    /// <param name="user"></param>
    /// <returns>List of risks</returns>
    /// <throws>UserNotAuthorizedException</throws>
    List<Risk> GetUserRisks(User user);
}