using Astrum.SharedLib.Domain.Entities;

namespace Astrum.ITDictionary.Aggregates;

public class Term: AggregateRootBase<Guid>
{
    public string Name { get; set; }

    public string Definition { get; set; }

    public Category Category { get; set; }

    public Guid CategoryId { get; set; }
}