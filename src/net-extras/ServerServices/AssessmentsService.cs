using DAL;
using DAL.Entities;

namespace ServerServices;
using ILogger = Serilog.ILogger;


public class AssessmentsService: ServiceBase, IAssessmentsService
{
    
    public AssessmentsService(ILogger logger, DALManager dalManager): base(logger, dalManager)
    {
        
    }
    
    public List<Assessment> List()
    {
        var srDbContext = DALManager.GetContext();
        
        return srDbContext.Assessments.ToList();
        
    }

    public Assessment? Get(int id)
    {
        var srDbContext = DALManager.GetContext();
        return srDbContext.Assessments.Find(id);
    }

    public List<AssessmentAnswer>? GetAnswers(int id)
    {
        var srDbContext = DALManager.GetContext();
        return srDbContext.AssessmentAnswers.Where(a => a.AssessmentId == id).ToList();
    }

    public List<AssessmentQuestion>? GetQuestions(int id)
    {
        var srDbContext = DALManager.GetContext();
        return srDbContext.AssessmentQuestions.Where(a => a.AssessmentId == id).ToList();
    }
    
}