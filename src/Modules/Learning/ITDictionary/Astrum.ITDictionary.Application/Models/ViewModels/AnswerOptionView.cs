namespace Astrum.ITDictionary.Models.ViewModels;

public class AnswerOptionView
{
    public Guid Id { get; set; }
    
    public Guid QuestionId { get; set; }

    public string Answer { get; set; }
}