using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Entities;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Astrum.TrackerProject.Application.Services;
using Astrum.TrackerProject.Application.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.TrackerProject.Backoffice.Controllers
{
    [Area("TrackerArticle")]
    [Route("[area]/[controller]")]
    public class TrackerArticleController : ApiBaseController
    {
        private readonly IArticleService _articleService;
        private readonly ISynchronisationService _synchronisationService;
        private readonly ILogHttpService _logHttpService; 

        public TrackerArticleController(IArticleService articleService, 
            ISynchronisationService synchronisationService, ILogHttpService logHttpService)
        {
            _articleService = articleService;
            _synchronisationService = synchronisationService;
            _logHttpService = logHttpService;
        }

        /// <summary>
        ///     Получение статей из базы знаний
        /// </summary>
        /// <returns></returns>
        [HttpGet("articles/{projectId}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(ArticleForm), StatusCodes.Status200OK)]
        public async Task<Result<List<ArticleForm>>> GetArticles([FromRoute] string projectId)
        {
            var articles = await _articleService.GetArticles(projectId);
            if(!articles.IsSuccess)
            {
                _logHttpService.Log(null, articles, HttpContext, "Получение статей", TypeRequest.GET,
                    ModuleAstrum.TrackerProject);
            }
            return Result.Success(articles);
        }

        /// <summary>
        ///     Получение статьи из базы знаний
        /// </summary>
        /// <param name="id">Id статьи</param>
        /// <returns></returns>
        [HttpGet("article/{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(ArticleForm), StatusCodes.Status200OK)]
        public async Task<Result<ArticleForm>> GetArticle([FromRoute] string id)
        {
            var article = await _articleService.GetArticle(id);
            if (!article.IsSuccess)
            {
                _logHttpService.Log(null, article, HttpContext, "Получение статьи", TypeRequest.GET,
                    ModuleAstrum.TrackerProject);
            }
            return Result.Success(article);
        }

        [HttpPost("synchronisearticles")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(IssueForm), StatusCodes.Status200OK)] 
        public async Task<Result> SynchroniseArticles(TrackerArticle article)
        {
            await _synchronisationService.SynchroniseArticleChanges(article);
            return Result.Success();
        }
    }
}
