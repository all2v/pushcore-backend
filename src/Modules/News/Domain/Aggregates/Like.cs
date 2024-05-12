using Astrum.SharedLib.Domain.Entities;

namespace Astrum.News.Aggregates;

public class Like : AggregateRootBase<Guid>
{
    public Like() { }

    public Like(Guid id)
    {
        Id = id;
    }

    public Guid PostId { get; set; }
    public Post Post { get; set; }
}