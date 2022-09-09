namespace Model.DTO.Statistics;

public class SecurityControlsStatistics
{
    public List<SecurityControlStatistic> SecurityControls { get; set; }
    public List<FrameworkStatistic>? FameworkStats { get; set; }

}