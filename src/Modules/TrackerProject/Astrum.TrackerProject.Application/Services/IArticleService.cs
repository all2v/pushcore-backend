using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.SharedLib.Common.Results;
using Astrum.TrackerProject.Application.ViewModels;

namespace Astrum.TrackerProject.Application.Services
{
    public interface IArticleService
    {
        Task<Result<List<ArticleForm>>> GetArticles(string projectId);
        Task<Result<ArticleForm>> GetArticle(string articleId);
        
    }
}
