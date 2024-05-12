using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Account.Features.Profile;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Application.Features.Profile.Queries
{
    public class GetUserPostsQuery : QueryResult<List<PostResponse>>
    {
        public GetUserPostsQuery(string username, int? startIndex, int? count)
        {
            Username = username;
            StartIndex = startIndex;
            Count = count;
        }

        public string Username { get; set; }
        public int? StartIndex { get; set; }
        public int? Count { get; set; }
    }
}
