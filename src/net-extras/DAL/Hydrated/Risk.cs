namespace DAL.Hydrated;

public class Risk: BaseHydrated
{

    private Entities.Risk _baseRisk;
    public string Source { get; set; }
    public Risk(Entities.Risk risk)
    {
        _baseRisk = risk;
        using (var context = _dalManager.GetContext())
        {
            Source = context.Sources.Where(s => s.Value == risk.Source).FirstOrDefault().Name;
        }

        
    }
}