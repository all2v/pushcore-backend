using Astrum.ITDictionary.Enums;

namespace Astrum.ITDictionary.Models.ViewModels;

public class TestStatisticsDetails
{
    public float SuccessRate { get; set; }

    public PracticeType PracticeType { get; set; }

    public List<TestStatistics> LastTestsStats { get; set; }
}