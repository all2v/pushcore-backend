using Astrum.SharedLib.Application.Extensions;
using Astrum.SharedLib.Domain.Entities;
using Astrum.SharedLib.Domain.Interfaces;

namespace Astrum.Articles.Aggregates;
//TODO: написать дочерние и родительские статьи. Сделать комментарии. Сделать вложения
public class ArticleContent
{
    public string? Text { get; set; }
    public string? Html { get; set; }
}

public class Article : AggregateRootBase<Guid>
{
    private Article() { }

    public Article(Guid id)
    {
        Id = id;
    }

    public string Name { get; set; }
    public string? Description { get; set; }
    public Category Category { get; set; }
    public Guid CategoryId { get; set; }
    public List<Tag> Tags { get; set; }
    public Author Author { get; set; }
    public Guid AuthorId { get; set; }
    public ArticleSlug Slug { get; set; }
    public int ReadingTime { get; set; }
    //public string? CoverUrl { get; set; }
    public Guid? CoverImageId { get; set; }
    public ArticleContent Content { get; set; }
    public string? TrackerArticleId { get; set; }
}

public class ArticleSlug
{
    public ArticleSlug() { }

    public ArticleSlug(string authorText, string nameText)
    {
        AuthorSlug = authorText.GenerateSlug();
        NameSlug = nameText.GenerateSlug();
    }

    public string? AuthorSlug { get; set; }
    public string? NameSlug { get; set; }
    public string FullSlug => string.Join('/', AuthorSlug, NameSlug);
}