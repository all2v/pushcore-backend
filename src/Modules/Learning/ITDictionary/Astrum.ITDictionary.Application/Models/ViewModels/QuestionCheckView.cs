namespace Astrum.ITDictionary.Models.ViewModels;

public class QuestionCheckView
{
    public Guid UserId { get; set; }

    public Guid QuestionId { get; set; }
    
    public bool CheckingResult { get; set; }
}