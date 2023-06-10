using System;
using System.Collections.Generic;
using DAL.Entities;
using ClientServices.Services;
using Serilog;
using Serilog.Core;

namespace ClientServices.Hydrated;

public class Risk: BaseHydrated
{
    private DAL.Entities.Risk _baseRisk;

    private IRisksService _risksService;
    
    private IUsersService _usersService;
    
    public Risk(DAL.Entities.Risk risk)
    {
        _baseRisk = risk;
        _risksService = GetService<IRisksService>();
        _usersService = GetService<IUsersService>();
    }

    public int Id => _baseRisk.Id;
    
    public string Status => _baseRisk.Status;

    public string Subject => _baseRisk.Subject;

    public string Source => _risksService.GetRiskSource(_baseRisk.Source);

    public string Category => _risksService.GetRiskCategory(_baseRisk.Category);

    public string Owner => _usersService.GetUserName(_baseRisk.Owner);
    
    public string SubmittedBy => _usersService.GetUserName(_baseRisk.SubmittedBy);

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