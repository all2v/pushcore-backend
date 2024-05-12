using Astrum.SharedLib.Domain.Entities;

namespace Astrum.ITDictionary.Aggregates;

public class Category : AggregateRootBase<Guid>
{
    public string Name { get; set; }
}