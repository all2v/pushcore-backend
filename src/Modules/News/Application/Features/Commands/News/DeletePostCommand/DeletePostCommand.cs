using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.News.Application.Features.Commands.News.DeletePostCommand
{
    public class DeletePostCommand : CommandResult<PostForm>
    {
        public Guid PostId { get; set; }

        public DeletePostCommand(Guid postId) 
        { 
            PostId = postId;
        }
    }
}
