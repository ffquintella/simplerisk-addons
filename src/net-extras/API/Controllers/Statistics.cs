using System.Linq.Expressions;
using System.Text;
using DAL;
using DAL.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Statistics;
using ILogger = Serilog.ILogger;

namespace API.Controllers;

[Authorize(Policy = "RequireValidUser")]
[ApiController]
[Route("[controller]")]
public class Statistics : ApiBaseController
{
    private readonly DALManager _dalManager;
    
    public Statistics(ILogger logger, DALManager dalManager) : base(logger)
    {
        _dalManager = dalManager;
    }
    

    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
    public ActionResult<List<string>> ListAvaliable()
    {
        Logger.Information("Listing avaliable statistics");
        return new List<string> { "RisksOverTime", "SecurityControls" };
    }
    
    [HttpGet]
    [Route("RisksOverTime")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
    public ActionResult<List<RisksOnDay>> GetRisksOverTime([FromQuery]int daysSpan = 30)
    {

        var firstDay = DateTime.Now.Subtract(TimeSpan.FromDays(daysSpan));
        
        var srDbContext = _dalManager.GetContext();
        var risks = srDbContext.Risks.Join(srDbContext.RiskScorings, 
            risk => risk.Id,
            riskScoring => riskScoring.Id,
            (risk, riskScoring) => new
            {
                Id = risk.Id,
                SubmissionDate = risk.SubmissionDate,
                CalculatedRisk = riskScoring.CalculatedRisk,
                Status = risk.Status
            }).
            Where(risk => risk.SubmissionDate > firstDay).ToList();
        
        var result = new List<RisksOnDay>();

        var computingDay = firstDay;

        while (computingDay < DateTime.Now)
        {
            var risksSelected = risks.Where(rsk => rsk.SubmissionDate.Date == computingDay.Date && rsk.Status != "Closed").ToList();

            var riskOnDay = new RisksOnDay
            {
                Day = computingDay,
                RisksCreated = 0,
                TotalRiskValue = 0
            };
            
            foreach (var risk in risksSelected)
            {
                riskOnDay.RisksCreated++;
                riskOnDay.TotalRiskValue += risk.CalculatedRisk;
            }
            result.Add(riskOnDay);
            computingDay = computingDay.AddDays(1);
        }

        return result;

    }
    
    [HttpGet]
    [Route("SecurityControls")]
    public ActionResult<SecurityControlsStatistics?> GetSecurityControls()
    {
      
        
        var srDbContext = _dalManager.GetContext();
       
        //risk.RiskCatalogMappings.Split(',').Select(int.Parse)
        
        var dbControls = srDbContext.Frameworks.Join(srDbContext.FrameworkControlMappings, 
                framework => framework.Value,
                frameworkControlMappings => frameworkControlMappings.Framework,
                (framework, frameworkControlMappings) => new
                {
                    Framework = framework.Name,
                    FrameworkId = framework.Value,
                    ControlId = frameworkControlMappings.ControlId,
                    ReferemceName = frameworkControlMappings.ReferenceName,
                }).Join(srDbContext.FrameworkControls,
                    frameworkControlMappings => frameworkControlMappings.ControlId,
                    frameworkControls => frameworkControls.Id,
                    (framework,  frameworkControls) => new
                    {
                        Framework = Encoding.UTF8.GetString(framework.Framework),
                        FrameworkId = framework.FrameworkId,
                        ControlId = framework.ControlId,
                        ReferemceName = framework.ReferemceName,
                        ControlName = frameworkControls.ShortName,
                        ClassId = frameworkControls.ControlClass,
                        MaturityId = frameworkControls.ControlMaturity,
                        DesireedMaturityId = frameworkControls.DesiredMaturity,
                        PiorityId = frameworkControls.ControlPriority,
                        Status = frameworkControls.Status,
                        Deleted = frameworkControls.Deleted,
                        ControlNumber = frameworkControls.ControlNumber,
                    }
                ).Where(sc => sc.Status == 1 && sc.Deleted == 0).ToList();

        var frameworkStats = dbControls.GroupBy(dc => dc.FrameworkId).Select(st => new
        {
            Framework = st.First().Framework,
            Count = st.Count()
        });
        
        /*var frameworkStats = srDbContext.Frameworks.Join(srDbContext.FrameworkControlMappings,
            framework => framework.Value,
            frameworkControlMappings => frameworkControlMappings.Framework,
            (framework, frameworkControlMappings) => new
            {
                Framework = framework.Name,
                FrameworkId = framework.Value,
                ControlId = frameworkControlMappings.ControlId,
                ReferemceName = frameworkControlMappings.ReferenceName,
            }).GroupBy(f => f.ControlId).Select(g => new
                {
                    Framework = g.First().Framework,
                    Count = g.Count()
                });*/
        
        
        var result = new SecurityControlsStatistics
        {
            SecurityControls = dbControls,
            FameworkStats = frameworkStats
        };
        
        return result;

    }
    
}