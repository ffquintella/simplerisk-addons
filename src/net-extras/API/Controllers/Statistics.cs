using DAL;
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
        var risks = srDbContext.Risks.Where(risk => risk.SubmissionDate > firstDay).ToList();
        
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
                
            }
            result.Add(riskOnDay);
            computingDay = computingDay.AddDays(1);
        }

        return result;

    }
    
}