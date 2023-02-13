using DAL.Entities;

namespace ServerServices;

public interface IRiskManagementService
{
    /// <summary>
    /// Lists all the risks the user has access to
    /// </summary>
    /// <param name="user"></param>
    /// <param name="status"> the risk status to use as filter</param>
    /// <returns>List of risks</returns>
    /// <throws>UserNotAuthorizedException</throws>
    List<Risk> GetUserRisks(User user, string? status);

    /// <summary>
    /// Gets all risks filtering optionaly by status
    /// </summary>
    /// <param name="status">the status to use as filter</param>
    /// <returns></returns>
    List<Risk> GetAll(string? status = null);


    /// <summary>
    /// Gets the risk category
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Category GetRiskCategory(int id);

    /// <summary>
    /// Gets all the risks that needs a mgmtReview
    /// </summary>
    /// <param name="status">Filter risk status</param>
    /// <returns>List of risks</returns>
    List<Risk> GetRisksNeedingReview(string? status = null);
}