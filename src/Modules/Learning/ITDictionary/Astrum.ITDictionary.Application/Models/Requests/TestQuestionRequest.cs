namespace Astrum.ITDictionary.Models.Requests;

public class TestQuestionRequest
{
    public Guid UserId { get; set; }

    // public Guid PracticeId { get; set; }

    public Guid TestQuestionId { get; set; }
    
    public Guid TestOptionId { get; set; }
}