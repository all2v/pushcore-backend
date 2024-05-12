using Astrum.Account.Services;
using Astrum.Appeal.Application.Services;
using Astrum.Appeal.Application.ViewModels;
using Astrum.Appeal.GraphQL;
using Astrum.Appeal.Services;
using Astrum.Appeal.ViewModels;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Appeal.Controllers;

/// <summary>
///     Appeal endpoints
/// </summary>
//[Authorize(Roles = "admin")]
[Route("[controller]")]
public class AppealController : ApiBaseController
{
    private readonly IAppealCategoryService _appealCategoryService;
    private readonly IAppealService _appealService;
    private readonly ITopicEventSender _sender;
    private readonly IUserProfileService _userProfileService;
    private readonly ILogHttpService _logger;

    /// <summary>
    ///     AppealController initializer
    /// </summary>
    /// <param name="appealService"></param>
    /// <param name="sender"></param>
    public AppealController(IAppealService appealService, IAppealCategoryService appealCategoryService,
        ITopicEventSender sender, IUserProfileService userProfileService, ILogHttpService logger)
    {
        _appealService = appealService;
        _appealCategoryService = appealCategoryService;
        _sender = sender;
        _userProfileService = userProfileService;
        _logger = logger;
    }

    /// <summary>
    ///     Создаёт новую заявку
    /// </summary>
    /// <param name="appealForm">Новая заявка</param>
    /// <returns></returns>
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(AppealFormResponse), StatusCodes.Status200OK)]
    public async Task<Result<AppealFormResponse>> Create([FromBody] AppealFormData appealForm)
    {
        var response = await _appealService.CreateAppeal(appealForm);
        _logger.Log(appealForm, response, HttpContext, "Создана заявка.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Appeal);
        return response;
    }

    /// <summary>
    ///     Обновляет заявку
    /// </summary>
    /// <param name="id">id заявки</param>
    /// <param name="appealForm">Изменённые поля заявки</param>
    /// <returns></returns>
    [HttpPut("{Id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(AppealFormResponse), StatusCodes.Status200OK)]
    public async Task<Result<AppealFormResponse>> Update(AppealForm appealForm)
    {
        var response = await _appealService.UpdateAppeal(appealForm);
        _logger.Log(appealForm, response, HttpContext, "Обновлена заявка.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Appeal);
        return response;
    }

    /// <summary>
    ///     Удаляет заявку
    /// </summary>
    /// <param name="id">id удаляемой заявки</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(AppealFormResponse), StatusCodes.Status200OK)]
    public async Task<Result<AppealFormResponse>> Delete([FromRoute] Guid id)
    {
        var response = await _appealService.DeleteAppeal(id);
        _logger.Log(id, response, HttpContext, "Удалена заявка.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.Appeal);
        return response;
    }

    /// <summary>
    ///     Создаёт категорию заявок
    /// </summary>
    /// <param name="name">Название категории</param>
    /// <returns></returns>
    [HttpPost("category/{name}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(AppealCategoryForm), StatusCodes.Status200OK)]
    public async Task<Result<AppealCategoryForm>> CreateCategory([FromRoute] string name)
    {
        var response = await _appealCategoryService.CreateCategory(name);
        _logger.Log(name, response, HttpContext, "Создана категория заявок.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Appeal);
        return response;
    }

    /// <summary>
    ///     Изменяет категорию
    /// </summary>
    /// <param name="id">id категории</param>
    /// <param name="name">Новое название заявки</param>
    /// <returns></returns>
    [HttpPut("category/{id}/{name}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(AppealCategoryForm), StatusCodes.Status200OK)]
    public async Task<Result<AppealCategoryForm>> UpdateCategory([FromRoute] Guid id, [FromRoute] string name)
    {
        var response = await _appealCategoryService.UpdateCategory(id, name);
        _logger.Log(name, response, HttpContext, "Категория заявки обновлена.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Appeal);
        return response;
    }

    /// <summary>
    ///     Удаляет заявку
    /// </summary>
    /// <param name="id">Id удаляемой заявки</param>
    /// <returns></returns>
    [HttpDelete("category/{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(AppealCategoryForm), StatusCodes.Status200OK)]
    public async Task<Result<AppealCategoryForm>> DeleteCategory([FromRoute] Guid id)
    {
        var response = await _appealCategoryService.DeleteCategory(id);
        _logger.Log(id, response, HttpContext, "Удалена категория заявки.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.Appeal);
        return response;
    }

    /// <summary>
    ///     Получает данные для страницы создания заявки
    /// </summary>
    [HttpGet]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(AppealCreatePageData), StatusCodes.Status200OK)]
    public async Task<Result<AppealCreatePageData>> GetAppealCreatePage()
    {
        var summaries = await _userProfileService.GetAllUsersProfilesSummariesAsync();
        var categories = await _appealCategoryService.GetAppealCategories();
        var data = new AppealCreatePageData(summaries, categories);
        return Result.Success(data);
    }
}