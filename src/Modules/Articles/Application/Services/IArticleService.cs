using Astrum.Articles.Application.Models.Requests;
using Astrum.Articles.Requests;
using Astrum.Articles.ViewModels;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Articles.Services;

public interface IArticleService
{
    public Task<Result<ArticleView>> Create(ArticleCreateRequest request);
    public Task<Result<ArticleView>> Update(Guid id, ArticleEditRequest request, CancellationToken cancellationToken = default);
    public Task<Result<List<ArticleSummary>>> GetAll(ArticlePredicate articlePredicate);
    public Task<Result<ArticleView>> GetById(Guid id);
    public Task<Result<ArticleView>> GetBySlug(string authorText, string nameText);
    public Task<Result<List<ArticleSummary>>> GetByAuthorId(Guid id);
    public Task<Result> Delete(Guid id);
    public Task<Result<SlugResult>> CheckSlug(string authorText, string nameText);
    public Task<Result> GenerateSlug();
}