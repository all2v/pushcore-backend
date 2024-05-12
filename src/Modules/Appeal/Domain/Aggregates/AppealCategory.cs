using Astrum.Appeal.Domain.Aggregates;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Appeal.Aggregates;

public class AppealCategory : AggregateRootBase<Guid>
{
    private AppealCategory()
    {
    }

    public AppealCategory(string category)
    {
        Category = category;
    }

    public string Category { get; set; }
    public List<AppealAppealCategory> Appeals { get; set; }
}