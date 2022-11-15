using DAL;
using DAL.Entities;
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

    public List<Risk> GetUserRisks(User user)
    {
        var risks = new List<Risk>();


        return risks;
    }
}