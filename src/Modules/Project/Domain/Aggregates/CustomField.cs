using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Projects.Aggregates;

public class CustomField : AggregateRootBase<Guid>
{
    private CustomField() { }

    public CustomField(Guid id)
    {
        Id = id;
    }

    public string Name { get; set; }
    public string Value { get; set; }
    public Guid ProjectId { get; set; }
}