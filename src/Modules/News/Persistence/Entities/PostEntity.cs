using Astrum.SharedLib.Persistence.Models;

namespace Astrum.News.Entities;

public class PostEntity : DataEntityBase<Guid>
{
    public string Title { get; set; }
    public string Text { get; set; }
    public bool IsArticle { get; set; }
    public Guid From { get; set; }
    public DateTimeOffset? Created { get; set; }
}