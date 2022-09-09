using System.Collections.Generic;
using Model.Statistics;

namespace GUIClient.Services;

public interface IStatisticsService
{
    List<RisksOnDay> GetRisksOverTime();
    SecurityControlsStatistics GetSecurityControlStatistics();

}