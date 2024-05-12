namespace Astrum.Articles.ViewModels;

public class CategoryInfo
{
    public CategoryView Category { get; set; }
    public List<ArticleCountByTag> Tags { get; set; }
}
