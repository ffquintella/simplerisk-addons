using DAL;
using Serilog;

namespace ServerServices;

public class RiskManagementService: IRiskManagementService
{
    private DALManager? _dalManager;
    private ILogger _log;

    public RiskManagementService(ILogger logger, DALManager dalManager)
    {
        _dalManager = dalManager;
        _log = logger;
    }
}