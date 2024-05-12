using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Articles.Requests;
using Microsoft.AspNetCore.Http;

namespace Astrum.Articles.Application.Models.Requests
{
    public class ArticleEditRequest
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Guid UserId { get; set; }
        public int ReadingTime { get; set; }
        public ArticleContentDto Content { get; set; }
        public Guid CategoryId { get; set; }
        public IFormFile? CoverImage { get; set; }
        public List<Guid> TagsId { get; set; } = new();
        public bool CreatePost { get; set; } = true;
    }
}
