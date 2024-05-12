using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.News.ViewModels;

namespace Astrum.News.DomainServices.ViewModels.Requests
{
    public class CommentRequest
    {
        public Guid PostId { get; set; }
        public Guid? ReplyCommentId { get; set; }
        public Guid From { get; set; }
        public string Text { get; set; }
        public DateTime? Created { get; set; }
    }
}
