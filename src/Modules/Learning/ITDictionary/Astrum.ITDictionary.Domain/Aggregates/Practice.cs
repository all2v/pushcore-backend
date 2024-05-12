using Astrum.ITDictionary.Enums;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.ITDictionary.Aggregates;

public class Practice : AggregateRootBase<Guid>
{
    public Guid UserId { get; set; }

    public PracticeType Type { get; set; }

    public bool IsFinished { get; set; }
}