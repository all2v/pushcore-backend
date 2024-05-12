using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Articles.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Articles.Application.Features.Queries.GetArticleBySlug
{
    public class GetArticleBySlugQuery : QueryResult<ArticleView>
    {
        public GetArticleBySlugQuery(string username, string articleName)
        {
            Username = username;
            ArticleName = articleName;
        }

        public string Username { get; set; }
        public string ArticleName { get; set; }
    }
}
