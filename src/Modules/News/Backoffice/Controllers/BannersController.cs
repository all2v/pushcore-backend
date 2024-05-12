using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Services;
using Astrum.News.Services;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Common.Results;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.News.Controllers;

/// <summary>
///     Banners endpoints
/// </summary>
//[Authorize(Roles = "admin")]
//[Route("[controller]")]
[Route("[controller]")]
public class BannersController : ApiBaseController
{
    private readonly IBannersService _bannerService;
    private readonly ITopicEventSender _sender;
    private readonly ILogHttpService _logger;

    /// <summary>
    ///     BannersController initializer
    /// </summary>
    /// <param name="newsService"></param>
    /// <param name="sender"></param>
    public BannersController(IBannersService bannerService, ITopicEventSender sender, ILogHttpService logger)
    {
        _bannerService = bannerService;
        _sender = sender;
        _logger = logger;
    }

    /// <summary>
    ///     Get active banners
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(List<BannerForm>), StatusCodes.Status200OK)]
    public async Task<Result<List<BannerForm>>> GetActiveBanners()
    {
        var response = await _bannerService.GetActiveBanners();
        return Result.Success(response);
    }

    /// <summary>
    ///     Create Banner entity
    /// </summary>
    /// <param name="banner"></param>
    /// <returns></returns>
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(BannerForm), StatusCodes.Status200OK)]
    public async Task<Result<BannerForm>> Create([FromBody] BannerForm banner)
    {
        var response = await _bannerService.CreateBanner(banner);
        _logger.Log(banner, response, HttpContext, "Создан баннер.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.News);
        return Result.Success(response);
    }

    /// <summary>
    ///     Update Banner entity
    /// </summary>
    /// <param name="id"></param>
    /// <param name="bannerForm"></param>
    /// <returns></returns>
    [HttpPut("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(BannerForm), StatusCodes.Status200OK)]
    public async Task<Result<BannerForm>> Update([FromRoute] Guid id, [FromBody] BannerForm bannerForm)
    {
        var response = await _bannerService.UpdateBanner(id, bannerForm);
        _logger.Log(bannerForm, response, HttpContext, "Обновлён баннер.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.News);
        return Result.Success(response);
    }

    /// <summary>
    ///     Delete Banner entity
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(BannerForm), StatusCodes.Status200OK)]
    public async Task<Result<BannerForm>> Delete([FromRoute] Guid id)
    {
        var response = await _bannerService.DeleteBanner(id);
        _logger.Log(id, response, HttpContext, "Удалён баннер.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.News);
        return Result.Success(response);
    }
}