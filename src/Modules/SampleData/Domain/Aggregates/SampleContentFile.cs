using Astrum.SharedLib.Domain.Entities;
namespace Astrum.SampleData.Aggregates;

public class SampleContentFile : AggregateRootBase<Guid>
{
    public Guid FileId { get; set; }
    public string ContextName { get; set; }
}
