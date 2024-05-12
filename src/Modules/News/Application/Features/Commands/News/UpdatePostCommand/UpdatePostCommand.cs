using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.News.Application.ViewModels.Requests;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.News.Application.Features.Commands.News.UpdatePostCommand
{
    public class UpdatePostCommand : CommandResult<PostForm>
    {
        public Guid PostId { get; set; }
        public PostRequest Post { get; set; }

        public UpdatePostCommand(Guid postId, PostRequest post) 
        {
            PostId = postId;
            Post = post;
        }
    }
}
