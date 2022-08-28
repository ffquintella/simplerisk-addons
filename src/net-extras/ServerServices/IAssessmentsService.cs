using DAL.Entities;

namespace ServerServices;

public interface IAssessmentsService
{
    /// <summary>
    /// List all assessments
    /// </summary>
    /// <returns></returns>
    List<Assessment> List();
}