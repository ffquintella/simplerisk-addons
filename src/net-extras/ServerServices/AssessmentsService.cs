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

    public Tuple<int,Assessment?> Create(Assessment assessment)
    {
        var srDbContext = DALManager.GetContext();
        try
        {
            var ass = srDbContext.Assessments.Add(assessment);

            if (ass.IsKeySet)
            {
                srDbContext.SaveChanges();
                return new Tuple<int, Assessment>(0, ass.Entity);
                
            }
        }
        catch (Exception ex)
        {
            Logger.Error("Error creating assessment: {0}", ex.Message);
            return new Tuple<int, Assessment>(1, null);
        }
        Logger.Error("Unkown error creating assessment");
        return new Tuple<int, Assessment?>(-1, null);
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