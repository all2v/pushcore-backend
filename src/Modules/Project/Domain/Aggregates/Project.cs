using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Projects.Aggregates;

public class Project : AggregateRootBase<Guid>
{
    private Project() { }

    public Project(Guid id)
    {
        Id = id;
    }

    public string Name { get; set; }
    public string? Description { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public Guid ProductId { get; set; }
    public List<Member>? Members { get; set; }
    public List<CustomField>? CustomFields { get; set; }
}