namespace Astrum.ITDictionary.Models.ViewModels;

public class TestStatistics
{
    public Guid Id { get; set; }
    
    public int QuestionsCount { get; set; }

    public int Correct { get; set; }

    public int Wrong { get; set; }
}