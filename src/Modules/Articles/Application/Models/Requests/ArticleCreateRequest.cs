using Astrum.Articles.Aggregates;
using Astrum.Storage.Application.ViewModels;
using Microsoft.AspNetCore.Http;

namespace Astrum.Articles.Requests
{
    public class ArticleCreateRequest
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid Author { get; set; }
        public int ReadingTime { get; set; }
        public ArticleContentDto Content { get; set; }
        public Guid CategoryId { get; set; }
        public IFormFile CoverImage { get; set; }
        public List<Guid> TagsId { get; set; } = new();
        public bool CreatePost { get; set; } = true;
    }    
}
