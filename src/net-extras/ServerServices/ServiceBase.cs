using DAL;
using ILogger = Serilog.ILogger;

namespace ServerServices;

public class ServiceBase
{
    protected ILogger Logger { get; }
    protected DALManager DALManager { get; }
    
    public ServiceBase(ILogger logger, DALManager dalManager)
    {
        Logger = logger;
        DALManager = dalManager;
    }
    
}