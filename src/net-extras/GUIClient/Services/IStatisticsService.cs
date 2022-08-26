using System.Collections.Generic;

namespace GUIClient.Services;

public interface IStatisticsService
{
    Dictionary<string, double> GetRisksOverTime();
    
}