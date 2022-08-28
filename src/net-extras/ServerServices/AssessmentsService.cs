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
    
}