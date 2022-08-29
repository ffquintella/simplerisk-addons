using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Entities;

namespace GUIClient.Services;

public interface IAssessmentsService
{
    /// <summary>
    /// Get the list of assessments frome the server
    /// </summary>
    /// <returns>The list or null</returns>
    Task<List<Assessment>?> GetAssessments();
}