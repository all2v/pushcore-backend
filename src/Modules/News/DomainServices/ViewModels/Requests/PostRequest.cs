using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.Attributes;
using Microsoft.AspNetCore.Http;

namespace Astrum.News.Application.ViewModels.Requests
{
    public class PostRequest
    {
        public string Title { get; set; }
        public string? Text { get; set; }
        public string? Description { get; set; }
        public int? ReadingTime { get; set; }
        public bool IsArticle { get; set; }
        public Guid From { get; set; }

        public DateTimeOffset? DateCreated { get; set; }

        //[NotMapped]
        //[DisplayName("Upload File")]
        //public IFormFile ImageFile { get; set; }
        public List<CommentForm> Comments { get; set; } = new();
        public List<LikeForm> Likes { get; set; } = new();
        [MaxFilesListSize(50 * 1024 * 1024)]
        public List<IFormFile> Attachments { get; set; } = new();
    }
}
