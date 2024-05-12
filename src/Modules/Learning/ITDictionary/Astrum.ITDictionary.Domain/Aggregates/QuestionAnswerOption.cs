using Astrum.SharedLib.Domain.Entities;

namespace Astrum.ITDictionary.Aggregates;

public class QuestionAnswerOption : AggregateRootBase<Guid>
{
    public Guid QuestionId { get; set; }

    public TestQuestion Question { get; set; }

    public Guid TermSourceId { get; set; }

    public Term TermSource { get; set; }

    public string Answer { get; set; }

    public bool IsCorrect { get; set; }
}