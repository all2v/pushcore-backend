using Astrum.SharedLib.Domain.Entities;

namespace Astrum.News.Aggregates;

public class Post : AggregateRootBase<Guid>
{
    public Post() { }

    public Post(Guid id)
    {
        Id = id;
    }

    public bool IsArticle { get; set; }
    public string Title { get; set; }
    public string? Text { get; set; }
    public string? Description { get; set; }
    public int? ReadingTime { get; set; }
    public IEnumerable<PostFileAttachment> FileAttachments { get; set; } = new List<PostFileAttachment>();
    public IEnumerable<Like> Likes { get; set; } = new List<Like>();
    public List<Comment> Comments { get; set; } = new List<Comment>();
}