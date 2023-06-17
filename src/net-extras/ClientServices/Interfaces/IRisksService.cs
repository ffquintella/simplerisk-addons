using DAL.Entities;

namespace ClientServices.Interfaces;

public interface IRisksService
{
    /// Returns the list of risks
    public List<Risk> GetAllRisks(bool includeClosed = false);
    
    public List<Risk> GetUserRisks();
    public string GetRiskCategory(int id);
    public List<Category>? GetRiskCategories();
    public string GetRiskSource(int id);
    
    public List<Source>? GetRiskSources();
    
    public List<RiskCatalog> GetRiskTypes(string ids,  bool all = false);
    
    public List<RiskCatalog> GetRiskTypes();
    
    public bool RiskSubjectExists(string status);

    public Risk? CreateRisk(Risk risk);
    
    public void SaveRisk(Risk risk);
    
    public void DeleteRisk(Risk risk);

}