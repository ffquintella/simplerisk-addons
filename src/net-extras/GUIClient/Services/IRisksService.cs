using System.Collections.Generic;
using DAL.Entities;

namespace GUIClient.Services;

public interface IRisksService
{
    public List<Risk> GetAllRisks();
    
    public List<Risk> GetUserRisks();

    public string GetRiskCategory(int id);
}