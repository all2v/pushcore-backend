using Astrum.Identity.Models;
using Astrum.SharedLib.Domain.Entities;

namespace Astrum.Account.Aggregates;

public class Achievement : AggregateRootBase<Guid>
{
    public string Name { get; set; }
    public Guid? IconId { get; set; }
    public string Description { get; set; }
    public virtual ICollection<UserProfile> Users { get; set; }
}