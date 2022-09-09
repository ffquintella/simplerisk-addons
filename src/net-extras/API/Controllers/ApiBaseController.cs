using Microsoft.AspNetCore.Mvc;
using ILogger = Serilog.ILogger;


namespace API.Controllers;

public class ApiBaseController: ControllerBase
{
    protected ILogger Logger;
    public ApiBaseController(ILogger logger)
    {
        Logger = logger;
    }
    
    
}