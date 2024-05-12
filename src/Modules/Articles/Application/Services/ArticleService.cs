using System.Text.RegularExpressions;
using Astrum.Articles.Aggregates;
using Astrum.Articles.Requests;
using Astrum.Articles.Repositories;
using Astrum.Articles.Specifications;
using Astrum.Articles.ViewModels;
using Astrum.Infrastructure.Integrations.YouTrack;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using AutoMapper;
using Astrum.Infrastructure.Integrations.YouTrack.Models.Article;
using Astrum.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Astrum.Articles.Application.Models.Requests;
using Astrum.Account.Services;
using static Duende.IdentityServer.Models.IdentityResources;
using Astrum.Logging.Services;
using MassTransit;
using Astrum.News.Services;
using HotChocolate.Data.Projections;
using Astrum.News.Application.Features.Commands.News.DeletePostCommand;
using MediatR;
using Astrum.News.GraphQL;
using HotChocolate.Subscriptions;
using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using Astrum.SharedLib.Application.Extensions;
using Microsoft.AspNetCore.Http;

namespace Astrum.Articles.Services;

public class ArticleService : IArticleService
{
    private readonly IArticleRepository _articleRepository;
    private readonly IArticleTagRepository _articleTagRepository;
    private readonly ICategoryService _categoryService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly INewsService _newsService;

    private readonly IMediator _mediator;
    private readonly ITopicEventSender _sender;
    private readonly IMapper _mapper;
    private readonly IFileStorage _fileStorage;

    private readonly IUserProfileService _userProfileService;

    public ArticleService(IArticleRepository articleRepository, IMapper mapper,
        IArticleTagRepository articleTagRepository, UserManager<ApplicationUser> userManager,
        IFileStorage fileStorage, ICategoryService categoryService, IUserProfileService userProfileService, INewsService newsService, IMediator mediator, ITopicEventSender sender)
    {
        _articleTagRepository = articleTagRepository;
        _articleRepository = articleRepository;
        _mapper = mapper;
        _fileStorage = fileStorage;
        _categoryService = categoryService;
        _userProfileService = userProfileService;
        _userManager = userManager;
        _newsService = newsService;
        _mediator = mediator;
        _sender = sender;
    }

    #region IArticleService Members

    public async Task<Result<ArticleView>> Create(ArticleCreateRequest request)
    {
        var validationResult = Validate(request.Name, request.ReadingTime, request.Description, request.Content.Text);
        if (validationResult.Failed)
            return Result.Error(validationResult.MessageWithErrors);

        var user = await _userManager.FindByIdAsync(request.Author.ToString());
        var username = user?.UserName;
        if (username == null)
            throw new Exception("User not found!");
        var profile = await _userProfileService.GetUserProfileAsync(request.Author);
        var slug = new ArticleSlug(username, request.Name);
        var validateSlug = await CheckSlugResult(slug);
        if (!validateSlug.IsFree)
            return Result.Error($"Название {validateSlug.Slug} уже занято.");

        var newArticle = _mapper.Map<Article>(request);
        newArticle.Slug = slug;
        var res = await _fileStorage.UploadFile(request.CoverImage);
        if (res is {Success: true})
        {
            newArticle.CoverImageId = res.UploadedFileId;
        }

        await _articleRepository.AddAsync(newArticle);
        try
        {
            await _articleRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при создании статьи.");
        }
        if (request.TagsId is not null && request.TagsId.Count > 0)
        {
            await _articleTagRepository.AddRangeAsync(request.TagsId.Select(e => new ArticleTag(e, newArticle.Id)));
            await _articleTagRepository.UnitOfWork.SaveChangesAsync();
        }
        var newArticleView = _mapper.Map<ArticleView>(newArticle);
        newArticleView.Author.Username = username;
        newArticleView.Author.Name = profile.Data.Name;
        newArticleView.Author.Surname = profile.Data.Surname;
        return Result.Success(newArticleView);
    }

    public async Task<Result<ArticleView>> Update(Guid id, ArticleEditRequest request, CancellationToken cancellationToken = default)
    {
        var validationResult = Validate(request.Name, request.ReadingTime, request.Description, request.Content.Text,
            request.CoverImage);
        if (validationResult.Failed)
            return Result.Error(validationResult.MessageWithErrors);

        var user = await _userManager.FindByIdAsync(request.UserId.ToString());
        var username = user?.UserName;
        if (username == null)
            throw new Exception("User not found!");
        var profile = await _userProfileService.GetUserProfileAsync(request.UserId);

        var spec = new GetArticleByIdSpec(id);
        var dbArticle = await _articleRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (dbArticle.Name != request.Name)
        {
            var slug = new ArticleSlug(username, request.Name);
            var validateSlug = await CheckSlugResult(slug);
            if (!validateSlug.IsFree)
            {
                return Result.Error($"Название {validateSlug.Slug} уже занято.");
            }
            dbArticle.Slug = slug;
        }
        //dbArticle.AuthorId = request.AuthorId;
        dbArticle.CategoryId = request.CategoryId;
        dbArticle.Name = request.Name;
        dbArticle.ReadingTime = request.ReadingTime;
        dbArticle.Content = _mapper.Map<ArticleContent>(request.Content);
        dbArticle.Description = request.Description;
        if (request.CoverImage != null)
        {
            var res = await _fileStorage.UploadFile(request.CoverImage);
            if (res is { Success: true }) 
                dbArticle.CoverImageId = res.UploadedFileId;
        }
        
        //dbArticle.Id = id;
        try
        {
            await _articleRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при обновлении статьи.");
        }
        //if (request.TagsId is not null && request.TagsId.Count > 0)
        //{
        //    await _articleTagRepository.AddRangeAsync(request.TagsId.Select(e => new ArticleTag(e, updatedArticle.Id)));
        //    await _articleTagRepository.UnitOfWork.SaveChangesAsync(cancellationToken);
        //}
        var updatedArticleView = _mapper.Map<ArticleView>(dbArticle);
        updatedArticleView.Author.Username = username;
        updatedArticleView.Author.Name = profile.Data.Name;
        updatedArticleView.Author.Surname = profile.Data.Surname;
        updatedArticleView.Category = await _categoryService.GetById(request.CategoryId);
        return Result.Success(updatedArticleView);
    }

    public async Task<Result> Delete(Guid id)
    {
        var spec = new GetArticleByIdSpec(id);

        var project = await _articleRepository.FirstOrDefaultAsync(spec);
        if (project == null)
            return Result.NotFound("Статья не найдена.");
        var slug = project.Slug.FullSlug;
        try
        {
            var posts = await _newsService.GetNews();
            Guid? postId = posts.Data.Posts.Where(post => post.IsArticle == true).
                Where(post => post.Text.Contains(slug)).Select(post => post.Id).FirstOrDefault();
            if (postId.HasValue)
            {
                var command = new DeletePostCommand(postId.Value);
                var response = await _mediator.Send(command);
                var news = await _newsService.GetNews();
                await _sender.SendAsync(nameof(SubscriptionNews.NewsChanged), news);
            }
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при удалении поста.");
        }
        try
        {
            await _articleRepository.DeleteAsync(project);
            await _articleRepository.UnitOfWork.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            return Result.Error(ex.Message, "Ошибка при удалении статьи.");
        }
        
        return Result.Success();
    }

    public async Task<Result<List<ArticleSummary>>> GetAll(ArticlePredicate articlePredicate)
    {
        var articles = await GetByPredicate(articlePredicate);
        //var spec = new GetArticlesDescSpec();
        //var articles = await _articleRepository.ListAsync(spec);
        var result = _mapper.Map<List<ArticleSummary>>(articles);
        foreach(var article in result) 
        {
            if (article.CoverImageId.HasValue)
                article.CoverUrl = await _fileStorage.GetFileUrl(article.CoverImageId.Value);
        }
        return Result.Success(result);
    }

    private async Task<List<Article>> GetByPredicate(ArticlePredicate articlePredicate)
    {
        var spec = new GetArticlesDescSpec();
        if (articlePredicate is null)
            return await _articleRepository.ListAsync(spec);
        IEnumerable<Article> query = await _articleRepository.ListAsync(spec);
        if (!string.IsNullOrWhiteSpace(articlePredicate.NamePredicate))
        {
            var predicate = articlePredicate.NamePredicate.ToUpper();
            query = query.Where(e => e.Name.ToUpper().Contains(predicate) 
                || e.Tags.Any(t=>t.Name.ToUpper().Contains(predicate)));
        }
        if (articlePredicate.TagId is not null && articlePredicate.TagId.Count > 0)
            query = query.Where(a=> a.Tags.Select(e => e.Id).Intersect(articlePredicate.TagId).Any());
        return query.ToList();
    }

    public async Task<Result<List<ArticleSummary>>> GetByAuthorId(Guid id)
    {
        var spec = new GetArticlesByAuthorIdSpec(id);
        var articles = await _articleRepository.ListAsync(spec);
        if (articles == null || articles.Count == 0)
            return Result.NotFound("Статьи не найдены.");
        var result = _mapper.Map<List<ArticleSummary>>(articles);
        foreach (var article in result)
        {
            if (article.CoverImageId.HasValue)
                article.CoverUrl = await _fileStorage.GetFileUrl(article.CoverImageId.Value);
        }
        return Result.Success(result);
    }

    public async Task<Result<ArticleView>> GetById(Guid id)
    {
        var spec = new GetArticleByIdSpec(id);
        var article = await _articleRepository.FirstOrDefaultAsync(spec);
        if (article == null)
            return Result.NotFound("Статья не найдена.");
        var result = _mapper.Map<ArticleView>(article);
        if (result.CoverImageId.HasValue)
            result.CoverUrl = await _fileStorage.GetFileUrl(article.CoverImageId.Value);
        return Result.Success(result);
    }

    public async Task<Result<ArticleView>> GetBySlug(string authorText, string nameText)
    {
        var slug = new ArticleSlug(authorText, nameText);
        var spec = new GetArticleBySlugSpec(slug);
        var article = await _articleRepository.FirstOrDefaultAsync(spec);
        if (article == null)
            return Result.NotFound("Статья не найдена.");
        var result = _mapper.Map<ArticleView>(article);
        if (result.CoverImageId.HasValue)
            result.CoverUrl = await _fileStorage.GetFileUrl(article.CoverImageId.Value);
        return Result.Success(result);
    }

    private async Task<SlugResult> CheckSlugResult(ArticleSlug slug)
    {
        var spec = new GetArticleBySlugSpec(slug);
        var isFree = !await _articleRepository.AnyAsync(spec);
        return new SlugResult()
        {
            IsFree = isFree,
            Slug = slug.FullSlug
        };
    }

    public async Task<Result<SlugResult>> CheckSlug(string authorText, string nameText)
    {
        var slug = new ArticleSlug(authorText, nameText);
        return Result.Success(await CheckSlugResult(slug));
    }

    public async Task<Result> GenerateSlug()
    {
        throw new NotImplementedException();
        // var spec = new GetArticlesDescSpec();
        // var articles = await _articleRepository.ListAsync(spec);
        // foreach(var article in articles)
        // {
        //     if (!string.IsNullOrWhiteSpace(article.Slug.NameSlug))
        //         continue;
        //     var user = await _userManager.FindByIdAsync(article.Author.UserId.ToString());
        //     var username = user?.UserName;
        //     var slug = new ArticleSlug(username, article.Name);
        //     var validateSlug = await CheckSlugResult(slug);
        //     if (!validateSlug.IsFree)
        //         slug.NameSlug += article.DateCreated.DateTime.ToString().GenerateSlug();
        //     article.Slug = slug;
        //     await _articleRepository.UpdateAsync(article);
        //     await _articleRepository.UnitOfWork.SaveChangesAsync();
        // }
        //
        // return Result.Success();
    }
    #endregion

    private static Result Validate(string name, int readingTime, string description, string content, IFormFile? image = null)
    {
        name = name.Trim();
        name = Regex.Replace(name, @"\s+", " ");
        var onlyLettersName = name.FilterSpecialChars();
        if (string.IsNullOrWhiteSpace(name) || name.Length > 30 || !name.Any(char.IsLetter) || name.Length < 5 ||
            onlyLettersName.Length < 5)
            return Result.Error("Недопустимое название статьи");

        description = description.Trim();
        description = Regex.Replace(description, @"\s+", " ");
        var onlyLettersDescription = description.FilterSpecialChars();
        if (string.IsNullOrWhiteSpace(description) || description.Length > 200 || !description.Any(char.IsLetter) ||
            description.Length < 20 || onlyLettersDescription.Length < 5)
            return Result.Error("Недопустимое описание статьи");

        content = content.Trim();
        content = Regex.Replace(content, @"\s+", " ");
        var onlyLettersContent = content.FilterSpecialChars();
        if (string.IsNullOrWhiteSpace(content) || content.Length < 100 || !content.Any(char.IsLetter) ||
            onlyLettersContent.Length < 5)
            return Result.Error("Недопустимое содержимое статьи");

        var availableFormats = new List<string> {"jpg", "png", "jpeg"};
        if (image != null && !availableFormats.Any(image.ContentType.Contains))
            return Result.Error("Недопустимый формат файла");

        if (readingTime <= 0)
            return Result.Error("Недопустимое время прочтения статьи");

        return Result.Success();
    }
}