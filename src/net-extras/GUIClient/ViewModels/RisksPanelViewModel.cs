using System.Collections.Generic;
using DAL.Entities;
using GUIClient.Services;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class RisksPanelViewModel: ViewModelBase
{
    private bool _initialized = false;
    private List<Risk> _risks;
    public List<Risk> Risks
    {
        get => _risks;
        set => this.RaiseAndSetIfChanged(ref _risks, value);
    }

    private IRisksService _risksService;
    
    public RisksPanelViewModel()
    {
        _risksService = GetService<IRisksService>();
    }
    
    public void Initialize()
    {
        if (!_initialized)
        {
            Risks = _risksService.GetUserRisks();
            _initialized = true;
        }
    }
}