using Astrum.Account.Features;
using Astrum.Account.Features.Achievement.Commands.AchievementDelete;
using Astrum.Account.Features.Achievement;
using Astrum.Account.Features.CustomField;
using Astrum.Account.Features.CustomField.Commands;
using Astrum.Account.Features.Profile;
using Astrum.Account.Features.Profile.Commands;
using Astrum.Account.Features.Profile.Queries;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Astrum.Account.Application.Features.Profile.Commands;
using Astrum.Storage.Application.ViewModels;
using Astrum.Account.Application.Features.Profile.Queries;
using Astrum.Account.Application.Features.Profile;
using Astrum.Account.Application.ViewModels;
using Astrum.News.ViewModels;
using Astrum.SharedLib.Application.Helpers;
using System.IdentityModel.Tokens.Jwt;
using Astrum.Account.Services;
using Astrum.Account.ViewModels;
using Astrum.Logging.Services;
using Astrum.Market.Application.ViewModels;
using Astrum.SharedLib.Application.Models.Filters;

namespace Astrum.Account.Controllers;

/// <summary>
/// </summary>
[Area("Account")]
[Route("[area]/[controller]")]
public class UserProfileController : ApiBaseController
{
    private readonly ILogHttpService _logger;
    private readonly IPositionsService _positionsService;

    public UserProfileController(ILogHttpService logger, IPositionsService positionsService)
    {
        _logger = logger;
        _positionsService = positionsService;
    }

    /// <summary>
    ///     Получить данные о пользователе
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    public async Task<Result<UserProfileResponse>> Get([FromRoute] string username)
    {
        var request = new GetUserProfileByUsernameQuery(username);
        var response = await Mediator.Send(request);
        if (!response.IsSuccess)
        {
            _logger.Log(username, response, HttpContext, "Получен пользователь по имени.", Logging.Entities.TypeRequest.GET, Logging.Entities.ModuleAstrum.Account);
        }
        return response;
    }

    /// <summary>
    ///     Получить посты пользователя
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("{username}/posts")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(List<PostResponse>), StatusCodes.Status200OK)]
    public async Task<Result<List<PostResponse>>> GetPosts([FromRoute] string username, 
        [FromQuery] int startIndex, [FromQuery] int count)
    {
        var request = new GetUserPostsQuery(username, startIndex, count);
        var response = await Mediator.Send(request);
        return response;
    }

    /// <summary>
    ///     Получить базовые данные о пользователе
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("basicinfo")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(BasicUserInfoResponse), StatusCodes.Status200OK)]
    public async Task<Result<BasicUserInfoResponse>> GetBasicUserInfo()
    {
        var userId = JwtManager.GetUserIdFromRequest(Request);
        if (userId == null)
            return Result.Error("Unable to parse token!");

        var request = new GetBasicUserInfoByIdQuery(userId.Value);
        var response = await Mediator.Send(request);
        return response;
    }

    /// <summary>
    ///     Получить полное имя пользователя
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("fullname/{username}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(FullNameResponce), StatusCodes.Status200OK)]
    public async Task<Result<FullNameResponce>> GetUserFullName([FromRoute] string username)
    {
        var request = new GetFullNameQuery(username);
        var response = await Mediator.Send(request);
        if (!response.IsSuccess)
        {
            _logger.Log(username, response, HttpContext, "Получен пользователь.", Logging.Entities.TypeRequest.GET, Logging.Entities.ModuleAstrum.Account);
        }
        return response;
    }

    /// <summary>
    ///     Проверить существование пользователя
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("exist/{username}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public async Task<Result<bool>> CheckUserExist([FromRoute] string username)
    {
        var request = new GetFullNameQuery(username);
        var response = await Mediator.Send(request);
        return response.Data != null;
    }

    /// <summary>
    ///     Получить данные для редактирования
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("edit/{username}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(EditUserProfileResponse), StatusCodes.Status200OK)]
    public async Task<Result<EditUserProfileResponse>> GetEditInfo([FromRoute] string username)
    {
        var request = new GetEditInfoByUsernameQuery(username);
        var response = await Mediator.Send(request);
        if (!response.IsSuccess)
        {
            _logger.Log(username, response, HttpContext, "Получен пользователь.", Logging.Entities.TypeRequest.GET, Logging.Entities.ModuleAstrum.Account);
        }
        return response;
    }

    /// <summary>
    ///     Получить краткую информацию о всех пользователях
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(List<UserProfileSummary>), StatusCodes.Status200OK)]
    public async Task<Result<List<UserProfileSummary>>> GetAll([FromQuery] string? name, [FromQuery] List<Guid>? positionIds)
    {
        var request = new GetUsersProfilesQuery(name, positionIds);
        return await Mediator.Send(request);
    }

    /// <summary>
    ///     Список должностей  
    /// </summary>  
    /// <returns></returns>
    [HttpGet("positions")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(EmployeeCategoryInfo), StatusCodes.Status200OK)]
    public async Task<Result<EmployeeCategoryInfo>> GetPositions()
    {
        var request = new GetPositionsInfoQuery();
        return await Mediator.Send(request);
    }

    /// <summary>
    ///     Список должностей  
    /// </summary>  
    /// <returns></returns>
    [HttpGet("positions/all")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(EmployeeCategoryInfo), StatusCodes.Status200OK)]
    public async Task<Result<List<PositionForm>>> GetAllPositions()
    {
        var result = await _positionsService.GetPositions();
        return Result.Success(result);
    }

    /// <summary>
    ///     Получение фильтров
    /// </summary>
    [HttpGet("filters")]
    [TranslateResultToActionResult]
    [ProducesResponseType(typeof(FilterResponse), StatusCodes.Status200OK)]
    public async Task<Result<FilterResponse>> GetFilters()
    {
        var request = new GetFiltersQuery();
        return await Mediator.Send(request);
    }

    /// <summary>
    ///     Изменить данные о пользователе
    /// </summary>
    /// <param name="username"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    [HttpPut("[action]/{username}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(EditUserProfileResponse), StatusCodes.Status200OK)]
    public async Task<Result<EditUserProfileResponse>> Edit([FromRoute] string username,
        [FromBody] EditUserProfileRequestBody body)
    {
        var command = Mapper.Map<EditUserProfileCommand>(body);
        command.Username = username;
        var response = await Mediator.Send(command);
        _logger.Log(username, response, HttpContext, "Пользователь изменён.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Account);
        return response;
    }

    /// <summary>
    ///     Изменить аватарку пользователя
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("[action]/{username}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<ChangeAvatarResponse>))]
    public async Task<Result<ChangeAvatarResponse>> ChangeAvatar([FromRoute] string username, IFormFile image)
    {
        var command = new ChangeAvatarCommand(username, image);
        var response = await Mediator.Send(command);
        _logger.Log(username, response, HttpContext, "Аватарка изменена.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Account);
        return response;
    }

    /// <summary>
    ///     Изменить обложку пользователя
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPut("[action]/{username}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<ChangeCoverResponse>))]
    public async Task<Result<ChangeCoverResponse>> ChangeCover([FromRoute] string username, IFormFile image)
    {
        var command = new ChangeCoverCommand(username, image);
        var response = await Mediator.Send(command);
        _logger.Log(username, response, HttpContext, "Обложка пользователя изменена.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Account);
        return response;
    }

    /// <summary>
    ///     Создать кастомное поле
    /// </summary>
    /// <returns></returns>
    [HttpPost("custom-field")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(CustomFieldResponse), StatusCodes.Status200OK)]
    public async Task<Result<CustomFieldResponse>> CreateCustomField([FromBody] CustomFieldCreateCommand command)
    {
        var response = await Mediator.Send(command);
        _logger.Log(command, response, HttpContext, "Создано кастомное поле.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Account);
        return response;
    }
    /// <summary>
    ///     Обновить кастомное поле
    /// </summary>
    /// <returns></returns>
    [HttpPut("custom-field")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(CustomFieldResponse), StatusCodes.Status200OK)]
    public async Task<Result<CustomFieldResponse>> EditCustomField([FromBody] CustomFieldEditCommand command)
    {
        var response = await Mediator.Send(command);
        _logger.Log(command, response, HttpContext, "Обновлено кастомное поле.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Account);
        return response;
    }
    /// <summary>
    ///     Удалить кастомное поле
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("custom-field/{id:guid}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(CustomFieldResponse), StatusCodes.Status200OK)]
    public async Task<Result<CustomFieldResponse>> DeleteCustomField([FromRoute] Guid id)
    {
        var request = new CustomFieldDeleteCommand(id);
        var response = await Mediator.Send(request);
        _logger.Log(id, response, HttpContext, "Удалено кастомное поле.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.Account);
        return response;
    }

    /// <summary>
    /// Уменьшить количество валюты у пользователя
    /// </summary>
    /// <param name="id">Id пользователя</param>
    /// <param name="moneyChange">Количественное изменение валюты</param>
    /// <returns></returns>
    [HttpPut("spend/{id}/{moneyChange}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    public async Task<Result<TransactionResponse>> SpendMoney([FromRoute] Guid id, [FromRoute] int moneyChange)
    {
        var command = new MoneyChangeCommand(id, moneyChange, true);
        var response = await Mediator.Send(command);
        _logger.Log(moneyChange, response, HttpContext, "Уменьшено количество валюты у пользователя.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Account);
        return response;
    }

    /// <summary>
    /// Увеличить количество валюты у пользователя
    /// </summary>
    /// <param name="id">Id пользователя</param>
    /// <param name="moneyChange">Количественное изменение валюты</param>
    /// <returns></returns>
    [HttpPut("replenish/{id}/{moneyChange}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(UserProfileResponse), StatusCodes.Status200OK)]
    public async Task<Result<TransactionResponse>> ReplenishMoney([FromRoute] Guid id, [FromRoute] int moneyChange)
    {
        var command = new MoneyChangeCommand(id, moneyChange, false);
        var response = await Mediator.Send(command);
        _logger.Log(moneyChange, response, HttpContext, "Увеличено количество валюты у пользователя.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Account); ;
        return response;
    }
}