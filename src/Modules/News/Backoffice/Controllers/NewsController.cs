using System.Security.Claims;
using System.Threading;
using Astrum.Account.Services;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Services;
using Astrum.News.Application.Features.Commands.News.DeletePostCommand;
using Astrum.News.Application.Features.Commands.News.UpdatePostCommand;
using Astrum.News.Application.ViewModels.Requests;
using Astrum.News.GraphQL;
using Astrum.News.Services;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.Results;
using Astrum.Storage.Services;
using AutoMapper;
using HotChocolate.Subscriptions;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;


namespace Astrum.News.Controllers;

/// <summary>
///     News endpoints
/// </summary>
//[Authorize(Roles = "admin")]
//[Area("News")]
[Route("[controller]")]
public class NewsController : ApiBaseController
{
    private readonly INewsService _newsService;
    private readonly ITopicEventSender _sender;
    private readonly IFileStorage _fileStorage;
    private readonly IMapper _mapper;
    private readonly IUserProfileService _userProfileService;
    private readonly ILogHttpService _logger;

    /// <summary>
    ///     NewsController initializer
    /// </summary>
    /// <param name="newsService"></param>
    /// <param name="sender"></param>
    public NewsController(INewsService newsService, ITopicEventSender sender, IFileStorage fileStorage,
        IMapper mapper, IUserProfileService userProfileService, ILogHttpService logger)
    {
        _newsService = newsService;
        _sender = sender;
        _fileStorage = fileStorage;
        _mapper = mapper;
        _userProfileService = userProfileService;
        _logger = logger;
    }

    /// <summary>
    ///     Create post entity
    /// </summary>
    /// <param name="post"></param>
    /// <returns></returns>
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(PostForm), StatusCodes.Status200OK)]
    public async Task<Result<PostForm>> Create([FromForm] PostRequest post)
    {
        var response = await _newsService.CreatePost(post);
        _logger.Log(post, response, HttpContext, "Создан пост.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.News);
        if (response.IsSuccess)
        {
            var news = await _newsService.GetNews();
            await _sender.SendAsync(nameof(SubscriptionNews.NewsChanged), news);
        }
        return response;
    }

    /// <summary>
    ///     Get news
    /// </summary>
    [HttpGet("/api/news/get-news")]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(NewsForm), StatusCodes.Status200OK)]
    public async Task<Result<NewsForm>> GetNews()
    {
        var response = await _newsService.GetNews();
        if (response != null)
            return Result.Success(response);
        return Result.Error(new string[] { "Unable to get news!" });
    }

    /// <summary>
    ///     Получение постов по стартовому индексу и количеству постов
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="startIndex"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    [HttpGet]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(PostPaginationView), StatusCodes.Status200OK)]
    public async Task<Result<PostPaginationView>> GetPostsByUserPagination([FromHeader] Guid userId, [FromHeader] int startIndex, [FromHeader] int count)
    {
        var response = await _newsService.GetPostsByUserPagination(userId, startIndex, count);
        return Result.Success(response);
    }

    /// <summary>
    ///     Update post entity
    /// </summary>
    /// <param name="id"></param>
    /// <param name="postForm"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(PostForm), StatusCodes.Status200OK)]
    public async Task<Result<PostForm>> Update([FromRoute] Guid id, [FromBody] PostRequest post)
    {
        var command = new UpdatePostCommand(id, post);
        var response = await Mediator.Send(command);
        _logger.Log(post, response, HttpContext, "Пост обновлён.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.News);
        var news = await _newsService.GetNews();
        await _sender.SendAsync(nameof(SubscriptionNews.NewsChanged), news);

        return response;
    }

    /// <summary>
    ///     Delete post entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(PostForm), StatusCodes.Status200OK)]
    public async Task<Result<PostForm>> Delete([FromRoute] Guid id)
    {
        var command = new DeletePostCommand(id);
        var response = await Mediator.Send(command);
        _logger.Log(id, response, HttpContext, "Пост удалён.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.News);
        var news = await _newsService.GetNews();
        await _sender.SendAsync(nameof(SubscriptionNews.NewsChanged), news);

        return response;
    }
}