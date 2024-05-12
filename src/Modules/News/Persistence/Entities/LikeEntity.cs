using Astrum.SharedLib.Persistence.Models;

namespace Astrum.News.Entities;

public class LikeEntity : DataEntityBase<Guid>
{
    public Guid PostId { get; set; }
    public PostEntity Post { get; set; }
    public Guid From { get; set; }
    public DateTimeOffset? Created { get; set; }
}