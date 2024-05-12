using Astrum.Appeal.Domain.Aggregates;
using Astrum.Appeal.Enums;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Appeal.Aggregates;

public class Appeal : AggregateRootBase<Guid>
{
    public Appeal() { }

    public Appeal(Guid id)
    {
        Id = id;
    }

    public string Title { get; set; }
    public string Request { get; set; }
    public List<AppealAppealCategory> AppealCategories { get; set; }

    public Guid From { get; set; }
    public Guid To { get; set; }
    public AppealStatus AppealStatus { get; set; }
    public string? Answer { get; set; }
    public DateTime? Closed { get; set; }
    public Guid? CoverImageId { get; set; }
}