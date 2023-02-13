using GUIClient.Services;

namespace GUIClient.Hydrated;

public class Risk: BaseHydrated
{
    private DAL.Entities.Risk _baseRisk;

    private IRisksService _risksService;
    
    public Risk(DAL.Entities.Risk risk)
    {
        _baseRisk = risk;
        _risksService = GetService<IRisksService>();
    }

    public int Id => _baseRisk.Id;
    
    public string Status => _baseRisk.Status;

    public string Subject => _baseRisk.Subject;

    public string Source => _risksService.GetRiskSource(_baseRisk.Source);

    public string Category => _risksService.GetRiskCategory(_baseRisk.Category);

}