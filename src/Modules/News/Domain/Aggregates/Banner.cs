using Astrum.SharedLib.Domain.Entities;

namespace Astrum.News.Aggregates;

public class Banner : AggregateRootBase<Guid>
{
    public Banner() { }

    public Banner(Guid id)
    {
        Id = id;
    }

    public string Title { get; set; }
    public bool IsActive { get; set; }
    public Guid? PictureId { get; set; }
}