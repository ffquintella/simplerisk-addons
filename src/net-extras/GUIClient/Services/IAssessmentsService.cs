using System;
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
    List<Assessment>? GetAssessments();
    
    /// <summary>
    /// Creates a new Assessment
    /// </summary>
    /// <param name="assessment"></param>
    /// <returns>0 if ok, -1 if error</returns>
    Tuple<int, Assessment?> Create(Assessment assessment);
    
    /// <summary>
    /// Get the assessment questions from the server
    /// </summary>
    /// <param name="assessmentId"></param>
    /// <returns>The question list or null if not found</returns>
    List<AssessmentQuestion>? GetAssessmentQuestions(int assessmentId);

    /// <summary>
    /// Get the assessment answers from the server
    /// </summary>
    /// <param name="assessmentId"></param>
    /// <returns></returns>
    List<AssessmentAnswer>? GetAssessmentAnswers(int assessmentId);
}