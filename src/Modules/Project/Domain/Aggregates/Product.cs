using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Projects.Aggregates;

public class Product : AggregateRootBase<Guid>
{
    private Product() { }

    public Product(Guid id)
    {
        Id = id;
    }

    public string Name { get; set; }
    public string? Description { get; set; }
    public Customer Customer { get; set; }
    public Guid CustomerId { get; set; }
    public DateTimeOffset StartDate { get; set; }
    public DateTimeOffset? EndDate { get; set; }
    public Guid CoverImageId { get; set; }
    public List<Project> Projects { get; set; }
    public int Index { get; set; }
}