using Astrum.ITDictionary.Enums;

namespace Astrum.ITDictionary.Models.Requests;

public class CreatePracticeRequest
{
    public Guid UserId { get; set; }

    public PracticeType Type { get; set; }

    public byte QuestionsCount { get; set; }
}