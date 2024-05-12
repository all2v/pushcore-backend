using Astrum.ITDictionary.Enums;

namespace Astrum.ITDictionary.Models.ViewModels;

public class TestView
{
    public Guid PracticeId { get; set; }

    public PracticeType Type { get; set; }

    public List<TestQuestionView> Questions { get; set; }

    public int QuestionsCount { get; set; }
}