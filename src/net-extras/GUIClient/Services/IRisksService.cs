using System.Collections.Generic;
using DAL.Entities;

namespace GUIClient.Services;

public interface IRisksService
{
    public List<Risk> GetAllRisks();
    
    public List<Risk> GetUserRisks();
    public string GetRiskCategory(int id);
    public List<Category>? GetRiskCategories();
    public string GetRiskSource(int id);
    
    public List<Source>? GetRiskSources();
    
    public List<RiskCatalog> GetRiskTypes(string ids,  bool all = false);
    
    public List<RiskCatalog> GetRiskTypes();
    
    public bool RiskSubjectExists(string status);

    public Risk? CreateRisk(Risk risk);

}