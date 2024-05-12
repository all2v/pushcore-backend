namespace Astrum.ITDictionary.Models.ViewModels;

public class StatisticsSummary
{
    public Guid UserId { get; set; }
    
    public int CountCompleted { get; set; }

    public List<PracticeView> Practices { get; set; }
}