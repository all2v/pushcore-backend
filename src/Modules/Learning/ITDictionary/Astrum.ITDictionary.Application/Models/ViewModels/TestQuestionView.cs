namespace Astrum.ITDictionary.Models.ViewModels;

public class TestQuestionView
{
    public Guid Id { get; set; }

    public Guid PracticeId { get; set; }

    public Guid TermSourceId { get; set; }

    public string Question { get; set; }

    public List<AnswerOptionView> AnswerOptions { get; set; }
}