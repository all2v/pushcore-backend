using Astrum.SharedLib.Domain.Entities;

namespace Astrum.News.Aggregates;

public class Comment : AggregateRootBase<Guid>
{
    public Comment() { }

    public Comment(Guid id)
    {
        Id = id;
    }

    public Guid PostId { get; set; }
    public Post Post { get; set; }
    public Guid? ReplyCommentId { get; set; }
    public Comment? ReplyComment { get; set; }
    public IEnumerable<Comment> ChildComments { get; set; } = new List<Comment>();
    public string Text { get; set; }
}