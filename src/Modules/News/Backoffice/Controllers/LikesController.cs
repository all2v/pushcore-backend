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
///     Likes endpoints
/// </summary>
//[Authorize(Roles = "admin")]
//[Area("News")]
//[Route("[controller]")]
[Route("[controller]")]
public class LikesController : ApiBaseController
{
    private readonly ILikesService _likeService;
    private readonly ITopicEventSender _sender;
    private readonly ILogHttpService _logger;

    /// <summary>
    ///     LikesController initializer
    /// </summary>
    /// <param name="likesService"></param>
    /// <param name="sender"></param>
    public LikesController(ILikesService likesService, ITopicEventSender sender, ILogHttpService logger)
    {
        _likeService = likesService;
        _sender = sender;
        _logger = logger;
    }

    /// <summary>
    ///     Create Like entity
    /// </summary>
    /// <param name="like"></param>
    /// <returns></returns>
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(LikeForm), StatusCodes.Status200OK)]
    public async Task<Result<LikeForm>> Create([FromBody] LikeRequest like)
    {
        var response = await _likeService.CreateLike(like);
        if (!response.IsSuccess)
            return Result.Error(response.Errors.ToArray());
        _logger.Log(like, response, HttpContext, "Добавлен лайк.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.News);
        return Result.Success(response);
    }

    /// <summary>
    ///     Delete Like entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(LikeForm), StatusCodes.Status200OK)]
    public async Task<Result<LikeForm>> DeleteById([FromRoute] Guid id)
    {
        var response = await _likeService.DeleteLike(id);
        if (!response.IsSuccess)
            return Result.Error(response.Errors.ToArray());
        _logger.Log(id, response, HttpContext, "Лайк убран.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.News);
        return Result.Success(response);
    }
}