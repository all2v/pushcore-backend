namespace Astrum.ITDictionary.Models.Requests;

public class FinishPracticeRequest
{
    public Guid PracticeId { get; set; }

    public Guid UserId { get; set; }
}