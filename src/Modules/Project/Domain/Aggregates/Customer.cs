using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Projects.Aggregates;

public class Customer : AggregateRootBase<Guid>
{
    private Customer() { }

    public Customer(Guid id)
    {
        Id = id;
    }

    public string Name { get; set; }
}