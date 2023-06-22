using System;
using System.Collections.Generic;
using DAL.Entities;
using Serilog;
using ClientServices.Interfaces;

namespace GUIClient.Hydrated;

public class Risk: BaseHydrated
{
    private DAL.Entities.Risk _baseRisk;

    private IRisksService _risksService;
    
    private IUsersService _usersService;
    
    private IMitigationService _mitigationService;
    
    public Risk(DAL.Entities.Risk risk)
    {
        _baseRisk = risk;
        _risksService = GetService<IRisksService>();
        _usersService = GetService<IUsersService>();
        _mitigationService = GetService<IMitigationService>();

    }

    public int Id => _baseRisk.Id;
    
    public string Status => _baseRisk.Status;

    public string Subject => _baseRisk.Subject;

    public string Source => _risksService.GetRiskSource(_baseRisk.Source);

    public string Category => _risksService.GetRiskCategory(_baseRisk.Category);

    public string Owner => _usersService.GetUserName(_baseRisk.Owner);
    
    public string SubmittedBy => _usersService.GetUserName(_baseRisk.SubmittedBy);
    
    public RiskScoring Scoring => _risksService.GetRiskScoring(_baseRisk.Id);

    private Mitigation? _mitigation;
    public Mitigation? Mitigation
    {
        get
        {
            if (_baseRisk.Status != "New")
            {
                if (_mitigation == null || _mitigation.RiskId != _baseRisk.Id)
                    _mitigation = _mitigationService.GetByRiskId(_baseRisk.Id);
            }
            else
            {
                _mitigation = null;
            }

            return _mitigation;
        }
    }

    public string Manager
    {
        get
        {
            try
            {
                return _usersService.GetUserName(_baseRisk.Manager);
            }
            catch (Exception ex)
            {
                Log.Warning("Error loading manager: {0}", ex.Message);
                return "";
            }
        }
    } 
    
    public List<RiskCatalog> Types => _risksService.GetRiskTypes(_baseRisk.RiskCatalogMapping);

}