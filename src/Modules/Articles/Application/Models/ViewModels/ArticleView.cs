using Astrum.Module.Articles.Application.ViewModels;

namespace Astrum.Articles.ViewModels
{
    public class ArticleView
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public AuthorView Author { get; set; }
        public CategoryView Category { get; set; }
        public DateTimeOffset DateCreated { get; set; }
        public int ReadingTime { get; set; }
        public string? CoverUrl { get; set; }
        public Guid? CoverImageId { get; set; }
        public List<TagView> Tags { get; set; }
        public string Content { get; set; }
        public string Slug { get; set; }
    }
}
