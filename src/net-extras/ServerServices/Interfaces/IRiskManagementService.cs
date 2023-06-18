using System.Collections.Generic;
using DAL.Entities;

namespace ServerServices.Interfaces;

public interface IRiskManagementService
{
    /// <summary>
    /// Lists all the risks the user has access to
    /// </summary>
    /// <param name="user"></param>
    /// <param name="status"> The risk status to use as filter</param>
    /// <param name="notStatus"> The risk status to use as not filter</param>
    /// <returns>List of risks</returns>
    /// <throws>UserNotAuthorizedException</throws>
    List<Risk> GetUserRisks(User user, string? status, string? notStatus = "Closed");

    /// <summary>
    /// Returns the risk with the given id
    /// </summary>
    /// <param name="id">Risk id</param>
    /// <returns>Risk object</returns>
    Risk GetRisk(int id);
    
    /// <summary>
    /// Gets the risk scoring
    /// </summary>
    /// <param name="id">Risk ID</param>
    /// <returns>Risk scoring object</returns>
    RiskScoring GetRiskScoring(int id);
    
    /// <summary>
    ///  Gets the risk with id if the user has permission 
    /// </summary>
    /// <param name="user">User object</param>
    /// <param name="id">id</param>
    /// <returns>Risk Object</returns>
    Risk GetUserRisk(User user,int id);
    
    /// <summary>
    /// Gets all risks filtering optionaly by status
    /// </summary>
    /// <param name="status">the status to use as filter</param>
    /// <returns></returns>
    List<Risk> GetAll(string? status = null, string? notStatus = "Closed");

    /// <summary>
    /// Check if subject exists
    /// </summary>
    /// <param name="subject"></param>
    /// <returns>bool</returns>
    bool SubjectExists(string subject);

    /// <summary>
    /// Create a new risk
    /// </summary>
    /// <param name="risk">the risk object to create</param>
    /// <returns>a risk object with updated fields</returns>
    Risk? CreateRisk(Risk risk);
    
    /// <summary>
    /// Saves the risk to the database
    /// </summary>
    /// <param name="risk">the risk object to save</param>
    void SaveRisk(Risk risk);
    
    /// <summary>
    /// Deletes the risk from the database
    /// </summary>
    /// <param name="id">The id of the risk to delete</param>
    void DeleteRisk(int id);
    
    /// <summary>
    /// Gets the risk category
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Category GetRiskCategory(int id);
    
    /// <summary>
    /// Gets the list of risk category
    /// </summary>
    /// <returns></returns>
    List<Category> GetRiskCategories();
    
    /// <summary>
    /// Gets the risk catalog item
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    RiskCatalog GetRiskCatalog(int id);

    /// <summary>
    /// Gets a list of risk catalogs
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    List<RiskCatalog> GetRiskCatalogs(List<int> ids);
    
    List<RiskCatalog> GetRiskCatalogs();
    
    /// <summary>
    /// Gets the risk source
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Source GetRiskSource(int id);

    /// <summary>
    /// List the risk sources
    /// </summary>
    /// <returns></returns>
    List<Source> GetRiskSources();
    
    /// <summary>
    /// Gets all the risks that needs a mgmtReview
    /// </summary>
    /// <param name="status">Filter risk status</param>
    /// <returns>List of risks</returns>
    List<Risk> GetRisksNeedingReview(string? status = null);
}