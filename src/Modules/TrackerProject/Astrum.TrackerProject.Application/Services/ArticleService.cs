using Astrum.Account.Services;
using Astrum.Infrastructure.Integrations.YouTrack;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Astrum.TrackerProject.Application.ViewModels;
using Astrum.TrackerProject.Domain.Aggregates;
using Astrum.TrackerProject.Domain.Repositories;
using Astrum.TrackerProject.Domain.Specification;
using AutoMapper;

namespace Astrum.TrackerProject.Application.Services
{
    public class ArticleService : IArticleService
    {
        private readonly ITrackerProjectRepository<Article> _repository;
        private readonly IMapper _mapper;
        private readonly IExternalUserService _externalUserService;
        private readonly IUserProfileService _profileService;

        public ArticleService(ITrackerProjectRepository<Article> repository, 
            IMapper mapper, IExternalUserService externalUserService, IUserProfileService profileService)
        {
            _repository = repository;
            _mapper = mapper;
            _externalUserService = externalUserService;
            _profileService = profileService;
        }

        public async Task<Result<List<ArticleForm>>> GetArticles(string projectId)
        {
            var spec = new GetArticleByProjectIdSpecification(projectId);
            var articles = await _repository.ListAsync(spec);
            if (articles.Count == 0)
            {
                return Result.Success();
            }
            var articleForms = _mapper.Map<List<ArticleForm>>(articles);
            var users = await _externalUserService.GetAllUserProfilesAsync();
            foreach (var form in articleForms)
            {
                form.Author = users.Data.FirstOrDefault(x => form.AuthorId == x.Id)?.UserProfileSummary;
            }

            return Result.Success(articleForms);
        }

        public async Task<Result<ArticleForm>> GetArticle(string articleId)
        {
            var spec = new GetArticleByIdSpecification(articleId);
            var article = await _repository.FirstOrDefaultAsync(spec);
            if (article == null)
            {
                return Result.Error("Статья по заданному id не найдена");
            }
            
            var articleForm = _mapper.Map<ArticleForm>(article);
            var childSpec = new GetChildArticlesByIdSpecification(article.ChildArticles);
            var childArticles = await _repository.ListAsync(childSpec);
            articleForm.ChildArticles = _mapper.Map<List<ArticleForm>>(childArticles);
            var users = await _externalUserService.GetAllUserProfilesAsync();
            articleForm.Author = users.Data.FirstOrDefault(x => articleForm.AuthorId == x.Id)?.UserProfileSummary;
            foreach (var articleFormComment in articleForm.Comments)
            {
                articleFormComment.Author = users.Data.FirstOrDefault(x => articleFormComment.AuthorId == x.Id)?
                    .UserProfileSummary;
            }

            return Result.Success(articleForm);
        }
    }
}
