using System.Collections.Generic;
using System.Collections.ObjectModel;
using DAL.Entities;
using GUIClient.Services;
using ReactiveUI;

namespace GUIClient.ViewModels;

public class RisksPanelViewModel: ViewModelBase
{
    private bool _initialized = false;

    private ObservableCollection<Risk> _risks;

    public ObservableCollection<Risk> Risks
    {
        get => _risks;
        set => this.RaiseAndSetIfChanged(ref _risks, value);
    }
    
    private IRisksService _risksService;
    
    public RisksPanelViewModel()
    {
        _risksService = GetService<IRisksService>();
        _risks =  new ObservableCollection<Risk>(new List<Risk>());
    }
    
    public void Initialize()
    {
        if (!_initialized)
        {
            Risks =  new ObservableCollection<Risk>(_risksService.GetUserRisks());
            //Risks = _risksService.GetUserRisks();
            _initialized = true;
        }
    }
}