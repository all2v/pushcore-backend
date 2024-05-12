using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Services;
using Astrum.News.DomainServices.ViewModels.Requests;
using Astrum.News.Services;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.Results;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.News.Controllers;

/// <summary>
///     Comments endpoints
/// </summary>
//[Authorize(Roles = "admin")]
//[Area("News")]
//[Route("[controller]")]
[Route("[controller]")]
public class CommentsController : ApiBaseController
{
    private readonly ICommentsService _commentService;
    private readonly ITopicEventSender _sender;
    private readonly ILogHttpService _logger;

    /// <summary>
    ///     CommentsController initializer
    /// </summary>
    /// <param name="commentsService"></param>
    /// <param name="sender"></param>
    public CommentsController(ICommentsService commentsService, ITopicEventSender sender, ILogHttpService logger)
    {
        _commentService = commentsService;
        _sender = sender;
        _logger = logger;
    }

    /// <summary>
    ///     Create Comment entity
    /// </summary>
    /// <param name="comment"></param>
    /// <returns></returns>
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(CommentForm), StatusCodes.Status200OK)]
    public async Task<Result<CommentForm>> Create([FromBody] CommentRequest comment)
    {
        var response = await _commentService.CreateComment(comment);
        _logger.Log(comment, response, HttpContext, "Создан комментарий.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.News);
        return Result.Success(response);
    }

    /// <summary>
    ///     Update Comment entity
    /// </summary>
    /// <param name="id"></param>
    /// <param name="commentForm"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(CommentForm), StatusCodes.Status200OK)]
    public async Task<Result<CommentForm>> Update([FromRoute] Guid id, [FromBody] CommentForm commentForm)
    {
        var response = await _commentService.UpdateComment(id, commentForm);
        _logger.Log(commentForm, response, HttpContext, "Обновлён комментарий.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.News);
        return Result.Success(response);
    }

    /// <summary>
    ///     Delete Comment entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(CommentForm), StatusCodes.Status200OK)]
    public async Task<Result<CommentForm>> Delete([FromRoute] Guid id)
    {
        var response = await _commentService.DeleteComment(id);
        _logger.Log(id, response, HttpContext, "Удалён комментарий.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.News);
        return Result.Success(response);
    }
}