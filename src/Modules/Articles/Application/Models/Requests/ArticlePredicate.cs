namespace Astrum.Articles.Requests;

public class ArticlePredicate
{
    public ArticlePredicate(string? namePredicate, List<Guid>? tagId)
    {
        NamePredicate = namePredicate;
        TagId = tagId;
    }

    public string? NamePredicate { get; set; }
    public List<Guid>? TagId { get; set; }
}
