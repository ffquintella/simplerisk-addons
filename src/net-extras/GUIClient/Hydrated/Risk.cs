namespace GUIClient.Hydrated;

public class Risk: BaseHydrated
{
    private DAL.Entities.Risk _baseRisk;
    public Risk(DAL.Entities.Risk risk)
    {
        _baseRisk = risk;
    }
}