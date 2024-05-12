using Astrum.SharedLib.Persistence.Models;

namespace Astrum.News.Entities;

public class CommentEntity : DataEntityBase<Guid>
{
    public Guid PostId { get; set; }
    public PostEntity Post { get; set; }
    public string Text { get; set; }
    public Guid From { get; set; }
    public DateTimeOffset? Created { get; set; }
}