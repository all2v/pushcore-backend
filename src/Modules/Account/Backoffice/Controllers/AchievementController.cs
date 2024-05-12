using Astrum.Account.Application.Features.Achievement.Commands.AchievementRemove;
using Astrum.Account.Application.Features.Achievement.Queries.GetAchievementsList;
using Astrum.Account.Features;
using Astrum.Account.Features.Achievement;
using Astrum.Account.Features.Achievement.Commands.AchievementAssign;
using Astrum.Account.Features.Achievement.Commands.AchievementCreate;
using Astrum.Account.Features.Achievement.Commands.AchievementDelete;
using Astrum.Account.Features.Achievement.Commands.AchievementUpdate;
using Astrum.Account.Features.Achievement.Queries.GetAchievement;
using Astrum.Account.Features.Achievement.Queries.GetAchievementsByUser;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Logging.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Controllers;

/// <summary>
///     Achievements endpoint
/// </summary>
[Area("Account")]
[Route("[area]/[controller]")]
public class AchievementController : ApiBaseController
{
    private readonly ILogHttpService _logger;
    public AchievementController(ILogHttpService logger)
    {
        _logger = logger;
    }
    /// <summary>
    ///     Получить описание достижения
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id:guid}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(AchievementResponse), StatusCodes.Status200OK)]
    public async Task<Result<AchievementResponse>> Get([FromRoute] Guid id)
    {
        var request = new GetAchievementQuery(id);
        var response = await Mediator.Send(request);
        if (!response.IsSuccess)
        {
            _logger.Log(id, response, HttpContext, "Достижение получено.", Logging.Entities.TypeRequest.GET, Logging.Entities.ModuleAstrum.Account);
        }
        return response;
    }

    /// <summary>
    ///     Получить список существующих достижений
    /// </summary>
    /// <returns></returns>
    [HttpGet("list")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(List<AchievementResponse>), StatusCodes.Status200OK)]
    public async Task<Result<List<AchievementResponse>>> GetAll()
    {
        var request = new GetAchievementsListQuery();
        return await Mediator.Send(request);
    }

    /// <summary>
    ///     Присвоить достижение пользователю
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("assign")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(UserAchievementResponse), StatusCodes.Status200OK)]
    public async Task<Result<UserAchievementResponse>> AssignAchievement([FromBody] AchievementAssignCommand command)
    {
        var response = await Mediator.Send(command);
        _logger.Log(command, response, HttpContext, "Достижение присвоено.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Account);
        return response;
    }

    /// <summary>
    ///     Удалить достижение у пользователя
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    [HttpPost("remove")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(UserAchievementResponse), StatusCodes.Status200OK)]
    public async Task<Result<UserAchievementResponse>> RemoveAchievement([FromBody] AchievementRemoveCommand command)
    {
        return await Mediator.Send(command);
    }

    /// <summary>
    ///     Получить достижения пользователя
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    [HttpGet("user/{username}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(List<AchievementResponse>), StatusCodes.Status200OK)]
    public async Task<Result<List<AchievementResponse>>> GetUserAchievementsByUsername([FromRoute] string username)
    {
        var request = new GetUserAchievementsByUsernameQuery(username);
        var response = await Mediator.Send(request);
        if (!response.IsSuccess)
        {
            _logger.Log(username, response, HttpContext, "Достижения получены.", Logging.Entities.TypeRequest.GET, Logging.Entities.ModuleAstrum.Account);
        }
        return response;
    }

    /// <summary>
    ///     Создать достижение
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(AchievementResponse), StatusCodes.Status200OK)]
    public async Task<Result<AchievementResponse>> Create([FromBody] AchievementCreateCommand request)
    {
        var response = await Mediator.Send(request);
        _logger.Log(request, response, HttpContext, "Создано достижение.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Account);
        return response;
    }

    /// <summary>
    ///     Изменить достижение
    /// </summary>
    /// <param name="id"></param>
    /// <param name="body"></param>
    /// <returns></returns>
    [HttpPut("{id:guid}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(AchievementResponse), StatusCodes.Status200OK)]
    public async Task<Result<AchievementResponse>> Update([FromRoute] Guid id,
        [FromBody] AchievementUpdateRequestBody body)
    {
        var command = Mapper.Map<AchievementUpdateCommand>(body);
        command.Id = id;
        var response = await Mediator.Send(command);
        _logger.Log(body, response, HttpContext, "Обновлено достижение.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Account);
        return response;
    }

    /// <summary>
    ///     Удалить достижение
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(AchievementResponse), StatusCodes.Status200OK)]
    public async Task<Result<AchievementResponse>> Delete([FromRoute] Guid id)
    {
        var request = new AchievementDeleteCommand(id);
        var response = await Mediator.Send(request);
        _logger.Log(id, response, HttpContext, "Удалено достижение.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.Account);
        return response;
    }
}