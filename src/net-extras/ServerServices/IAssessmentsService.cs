using DAL.Entities;

namespace ServerServices;

public interface IAssessmentsService
{
    /// <summary>
    /// List all assessments
    /// </summary>
    /// <returns>List of Assessments</returns>
    List<Assessment> List();
    
    /// <summary>
    /// Returns one assessment by id
    /// </summary>
    /// <param name="id">Assessment Id</param>
    /// <returns>Assessment Object Or Null if not found</returns>
    Assessment? Get(int id);
    
    /// <summary>
    /// Returns the list of answers from one assessment by id
    /// </summary>
    /// <param name="id">Assessment Id</param>
    /// <returns>AssessmentAnswer List Or Null if not found</returns>
    List<AssessmentAnswer>? GetAnswers(int id);
    
    /// <summary>
    /// Returns the list of questions from one assessment by id
    /// </summary>
    /// <param name="id">Assessment Id</param>
    /// <returns>AssessmentQuestion List Or Null if not found</returns>
    List<AssessmentQuestion>? GetQuestions(int id);

    /// <summary>
    /// Creates a new assessment on the database
    /// </summary>
    /// <param name="assessment"></param>
    /// <returns>-1 if error, 0 if ok, 1 if already exists</returns>
    Tuple<int,Assessment?> Create(Assessment assessment);
}