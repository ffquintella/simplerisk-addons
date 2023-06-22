using AutoMapper;
using DAL;
using DAL.Entities;
using Model.Exceptions;
using Serilog;
using ServerServices.Interfaces;

namespace ServerServices.Services;

public class MitigationManagementService: IMitigationManagementService
{
    private DALManager _dalManager;
    private ILogger _log;
    private readonly IRoleManagementService _roleManagement;
    private IMapper _mapper;

    public MitigationManagementService(
        ILogger logger, 
        DALManager dalManager,
        IMapper mapper,
        IRoleManagementService roleManagementService
    )
    {
        _dalManager = dalManager;
        _log = logger;
        _roleManagement = roleManagementService;
        _mapper = mapper;
    }
    
    public Mitigation GetById(int id)
    {
        using (var context = _dalManager.GetContext())
        {
            var mitigation = context.Mitigations.FirstOrDefault(m => m.Id == id);
            if (mitigation == null)
            {
                Log.Error("Mitigation with id {id} not found", id);
                throw new DataNotFoundException("Mitigation", id.ToString());
            }

            return mitigation;
        }
    }
    
    public Mitigation GetByRiskId(int id)
    {
        using (var context = _dalManager.GetContext())
        {
            var mitigation = context.Mitigations.FirstOrDefault(m => m.RiskId == id);
            if (mitigation == null)
            {
                Log.Error("Mitigation with id {id} not found", id);
                throw new DataNotFoundException("Mitigation", id.ToString());
            }

            return mitigation;
        }
    }

    public List<PlanningStrategy> ListStrategies()
    {
        using (var context = _dalManager.GetContext())
        {
            var strategies = context.PlanningStrategies.ToList();
            if (strategies == null)
            {
                Log.Error("Error Listing strategies");
                throw new DataNotFoundException("PlanningStrategies", "");
            }

            return strategies;
        }
    }
}