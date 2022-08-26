using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model.Statistics;

namespace API.Controllers;

[Authorize(Policy = "RequireValidUser")]
[ApiController]
[Route("[controller]")]
public class Statistics : ControllerBase
{
    [HttpGet]
    [Route("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
    public ActionResult<List<string>> ListAvaliable()
    {
        return new List<string> { "RisksOverTime", "SecurityControls" };
    }
    
    [HttpGet]
    [Route("RisksOverTime")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<string>))]
    public ActionResult<List<RisksOnDay>> GetRisksOverTime()
    {
        return new List<RisksOnDay>
        {
            new RisksOnDay
            {
                Day = new DateTime(2022,1,1),
                RisksCreated = 5,
                TotalRiskValue = 22.5f
            },
            new RisksOnDay
            {
                Day = new DateTime(2022,1,2),
                RisksCreated = 1,
                TotalRiskValue = 2.5f
            },
            new RisksOnDay
            {
                Day = new DateTime(2022,1,3),
                RisksCreated = 15,
                TotalRiskValue = 45.5f
            },
            new RisksOnDay
            {
                Day = new DateTime(2022,1,4),
                RisksCreated = 8,
                TotalRiskValue = 32.3f
            },
            
        };
    }
    
}