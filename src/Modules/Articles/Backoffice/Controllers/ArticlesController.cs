using Astrum.Account.Services;
using Astrum.Articles.Services;
using Astrum.Articles.ViewModels;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using AutoMapper;
using Astrum.SharedLib.Common.Results;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Astrum.News.Services;
using MassTransit;
using Astrum.Articles.Requests;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Project;
using Newtonsoft.Json;
using Astrum.Articles.Application.Features.Commands.Articles.UpdateArticleCommand;
using Astrum.Articles.Application.Models.Requests;
using Astrum.Logging.Services;
using Astrum.Articles.Application.Features.Queries.GetArticleBySlug;
using Astrum.SharedLib.Application.Models.Filters;

namespace Astrum.Articles.Controllers
{
    [Route("[controller]")]
    public class ArticlesController : ApiBaseController
    {
        private readonly IArticleService _articleService;
        private readonly ICategoryService _categoryService;
        private readonly INewsService _newsService;
        private readonly ITopicEventSender _sender;
        private readonly IUserProfileService _userProfileService;
        private readonly IMapper _mapper;
        private readonly ILogHttpService _logger;

        public ArticlesController(IArticleService articleService, ICategoryService categoryService,
            ITopicEventSender sender, IUserProfileService userProfileService, IMapper mapper
            , INewsService newsService, ILogHttpService logger)
        {
            _newsService = newsService;
            _articleService = articleService;
            _categoryService = categoryService;
            _sender = sender;
            _userProfileService = userProfileService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        ///     Создать статью
        /// </summary>
        [HttpPost]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(ArticleView), StatusCodes.Status200OK)]
        public async Task<Result<ArticleView>> Create([FromForm] ArticleCreateRequest articleRequest)
        {
            var response = await _articleService.Create(articleRequest);
            _logger.Log(articleRequest, response, HttpContext, "Создана статья.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Articles);
            if (!response.IsSuccess)
                return response;
            var article = response.Data;
            if (!articleRequest.CreatePost)
                return await GetById(article.Id);
            await _newsService.CreatePost(new News.Application.ViewModels.Requests.PostRequest()
            {
                DateCreated = DateTime.Now,
                From = articleRequest.Author,
                IsArticle = true,
                Title = article.Name,
                Text = "Статья доступна по ссылке - " + GetApiPath() + "/" + article.Slug,
                Description = article.Description,
                ReadingTime = article.ReadingTime
            });
            return await GetById(article.Id);
        }
        /// <summary>
        /// Обновить статью
        /// </summary>
        /// <param name="id"></param>
        /// <param name="articleRequest"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(ArticleView), StatusCodes.Status200OK)]
        public async Task<Result<ArticleView>> Update([FromRoute] Guid id, [FromForm] ArticleEditRequest articleRequest)
        {
            var command = new UpdateArticleCommand(id, articleRequest);
            var result = await Mediator.Send(command);
            _logger.Log(articleRequest, result, HttpContext, "Статья обновлена.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Articles);
            return result;
        }

        /// <summary>
        ///     Список статей
        /// </summary>
        /// <param name="predicate">
        ///     предикат(опционально)
        /// </param>
        ///<param name = "tagId">
        ///     тэги(опционально)
        /// </param> 
        [HttpGet]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(List<ArticleSummary>), StatusCodes.Status200OK)]
        public async Task<Result<List<ArticleSummary>>> GetAll([FromQuery] string? predicate, [FromQuery] List<Guid>? tagId )
        {
            var predicateObject = new ArticlePredicate(predicate, tagId);
            var response = await _articleService.GetAll(predicateObject);
            if (!response.IsSuccess)
                return response;
            var articles = response.Data;
            var authors = articles.Select(a => a.Author.UserId).Distinct();
            var authorsSummary = (await _userProfileService.GetUsersProfilesSummariesAsync(authors)).Data
                .ToDictionary(c => c.UserId, v => v);
            articles.ForEach(a => a.Author = authorsSummary.ContainsKey(a.Author.UserId) ?
            _mapper.Map(authorsSummary[a.Author.UserId], a.Author) : null);
            return response;
        }

        /// <summary>
        ///     Получить статью по id
        /// </summary>
        [HttpGet("{id}")] 
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(ArticleView), StatusCodes.Status200OK)]
        public async Task<Result<ArticleView>> GetById([FromRoute] Guid id)
        {
            var response = await _articleService.GetById(id);
            if (!response.IsSuccess)
            {
                _logger.Log(id, response, HttpContext, "Статья получена.", Logging.Entities.TypeRequest.GET, Logging.Entities.ModuleAstrum.Articles);
                return response;
            }  
            var article = response.Data;
            var author = await _userProfileService
                .GetUserProfileSummaryAsync(article.Author.UserId);
            article.Author = _mapper.Map(author.Data, article.Author);
            return response;
        }

        /// <summary>
        ///     Получить статью по slug
        /// </summary>
        [HttpGet("{userName}/{articleName}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(ArticleView), StatusCodes.Status200OK)]
        public async Task<Result<ArticleView>> GetBySlug([FromRoute] string userName, [FromRoute] string articleName)
        {
            var query = new GetArticleBySlugQuery(userName, articleName);
            var response = await Mediator.Send(query);

            if (!response.IsSuccess)
                _logger.Log(userName + ", " + articleName, response, HttpContext, "Статья получена.", Logging.Entities.TypeRequest.GET, Logging.Entities.ModuleAstrum.Articles);

            return response;
        }

        /// <summary>
        ///     Получить статью по id автора
        /// </summary>
        [HttpGet("author/{authorId}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(List<ArticleSummary>), StatusCodes.Status200OK)]
        public async Task<Result<List<ArticleSummary>>> GetByAuthor([FromRoute] Guid authorId)
        {
            var response = await _articleService.GetByAuthorId(authorId);
            if (!response.IsSuccess)
            {
                _logger.Log(authorId, response, HttpContext, "Статья получена.", Logging.Entities.TypeRequest.GET, Logging.Entities.ModuleAstrum.Articles);
                return response;
            }
            var articles = response.Data;
            var author = articles.FirstOrDefault().Author;
            var authorSummary = await _userProfileService
                .GetUserProfileSummaryAsync(author.UserId);
            var authorView = _mapper.Map(authorSummary, author);
            articles.ForEach(a => a.Author = authorView);
            return response;
        }

        /// <summary>
        ///     Список категорий
        /// </summary>
        [HttpGet("categories")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        [ProducesResponseType(typeof(List<CategoryView>), StatusCodes.Status200OK)]
        public async Task<Result<List<CategoryView>>> GetCategories()
        {
            return await _categoryService.GetAll();
        }

        /// <summary>
        ///     Количество статей по тэгам
        /// </summary>
        [HttpGet("categories/info")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(List<CategoryInfo>), StatusCodes.Status200OK)]
        public async Task<Result<List<CategoryInfo>>> GetCategoriesInfo()
        {
            var res = await _categoryService.GetInfo();
            return res;
        }

        /// <summary>
        ///     Получение фильтров
        /// </summary>
        [HttpGet("filters")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(FilterResponse), StatusCodes.Status200OK)]
        public async Task<Result<FilterResponse>> GetFilters()
        {
            var res = await _categoryService.GetFilters();
            return res;
        }

        /// <summary>
        ///     Удалить статью по id
        /// </summary>
        [HttpDelete("{id}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result))]
        public async Task<Result> Delete([FromRoute] Guid id)
        {
            var response = await _articleService.Delete(id);
            _logger.Log(id, response, HttpContext, "Статья удалена", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.Articles);
            return response;
        }

        /// <summary>
        ///    Проверить доступность имени статьи
        /// </summary>
        [HttpGet("check/{userName}/{articleName}")]
        [TranslateResultToActionResult]
        [ProducesDefaultResponseType(typeof(Result<SlugResult>))]
        [ProducesResponseType(typeof(List<SlugResult>), StatusCodes.Status200OK)]
        public async Task<Result<SlugResult>> CheckSlug([FromRoute] string userName, [FromRoute] string articleName)
        {
            var response = await _articleService.CheckSlug(userName, articleName);
            if (response.Failed)
                _logger.Log(userName + ", " + articleName, response, HttpContext, "Доступность имени проверена.",
                    Logging.Entities.TypeRequest.GET, Logging.Entities.ModuleAstrum.Articles);
            return response;
        }

        private string GetApiPath()
        {
            var request = HttpContext.Request;
            return request.Scheme + "://" + request.Host + "/articles";
        }

        /// <summary>
        ///    сгенерировать слаг если нет
        /// </summary>
        [HttpPost("generateslug")]
        [TranslateResultToActionResult]
        [ProducesResponseType(typeof(List<Result>), StatusCodes.Status200OK)]
        public async Task<Result> GenerateSlug()
        {
            return await _articleService.GenerateSlug();
        }
    }
}
