using Astrum.Account.Services;
using Astrum.Infrastructure.Integrations.YouTrack;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Module.Project.Application.ViewModels.DTO;
using Astrum.Projects.Services;
using Astrum.Projects.ViewModels;
using Astrum.Projects.ViewModels.DTO;
using Astrum.Projects.ViewModels.Requests;
using Astrum.Projects.ViewModels.Views;
using AutoMapper;
using Astrum.SharedLib.Common.Results;
using HotChocolate.Subscriptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Astrum.Logging.Services;
using Microsoft.AspNetCore.Authorization;

namespace Astrum.Projects.Controllers;

[Route("[controller]")]
public class ProjectsController : ApiBaseController
{
    private readonly IProjectService _projectService;
    private readonly IUserProfileService _userProfileService;
    private readonly IMapper _mapper;
    private readonly ITopicEventSender _sender;
    private readonly ILogHttpService _logger;

    public ProjectsController(ITopicEventSender sender, IProjectService projectService
        , IUserProfileService userProfileService, IMapper mapper, ILogHttpService logger)
    {
        _sender = sender;
        _projectService = projectService;
        _userProfileService = userProfileService;
        _mapper = mapper;
        _logger = logger;
    }

    /// <summary>
    ///     Создать проект
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Manager,Admin,SuperAdmin")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(ProjectView), StatusCodes.Status200OK)]
    public async Task<Result<ProjectView>> Create([FromBody] ProjectRequest project)
    {
        var response = await _projectService.Create(project);
        _logger.Log(project, response, HttpContext, "Проект создан.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Project);
        return response;
    }


    /// <summary>
    /// Список проектов
    /// </summary>
    //[HttpPost("getAll")]
    //[ProducesResponseType(typeof(List<ProjectView>), StatusCodes.Status200OK)]
    //public async Task<IActionResult> GetAll()
    //{
    //    var projects = await _projectService.GetAll();
    //    var response = Result.Success(projects);
    //    return response;
    //}

    /// <summary>
    ///     Удалить проект по id
    /// </summary>
    [HttpDelete("{id}")]
    [Authorize(Roles = "Manager,Admin,SuperAdmin")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    public async Task<Result<ProjectView>> Delete([FromRoute] Guid id)
    {
        var response = await _projectService.Delete(id);
        _logger.Log(id, response, HttpContext, "Проект удалён.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.Project);
        return response;
    }

    /// <summary>
    ///     Обновить проект
    /// </summary>
    [HttpPut("{id:guid}")]
    [Authorize(Roles = "Manager,Admin,SuperAdmin")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(ProjectUpdateDto), StatusCodes.Status200OK)]
    public async Task<Result<ProjectView>> Update(Guid id, [FromBody] ProjectUpdateDto project)
    {
        var response = await _projectService.Update(id, project);
        _logger.Log(project, response, HttpContext, "Проект обновлён.", Logging.Entities.TypeRequest.PUT, Logging.Entities.ModuleAstrum.Project);
        return response;
    }

    /// <summary>
    ///     Получить проект по id
    /// </summary>
    [HttpGet("get/{id}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(ProjectView), StatusCodes.Status200OK)]
    public async Task<Result<ProjectView>> Get([FromRoute] Guid id)
    {
        var response = await _projectService.GetWithProductName(id);
        if (!response.IsSuccess)
            return response;
        var product = response.Data;
        var members = await _userProfileService
            .GetUsersProfilesSummariesAsync(product.Members.Select(e => e.UserId));

        product.Members = product.Members.Join(members.Data,
            member => member.UserId,
            profile => profile.UserId, 
            (member, profile) => _mapper.Map(profile, member))
        .ToList();
        if (!response.IsSuccess)
        {
            _logger.Log(id, response, HttpContext, "Проект получен.", Logging.Entities.TypeRequest.GET, Logging.Entities.ModuleAstrum.Project);
        }
        return response;
    }

    /// <summary>
    ///     Добавить участников в проект
    /// </summary>
    [HttpPost("addMembers")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(ProjectView), StatusCodes.Status200OK)]
    public async Task<Result<ProjectView>> AddMembers([FromBody] AddMembersDto addMembersDto)
    {
        var response = await _projectService.AddMembers(addMembersDto);
        _logger.Log(addMembersDto, response, HttpContext, "Добавлены члены проекта.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Project);
        return response;
    }

    /// <summary>
    ///     Удалить участников из проекта
    /// </summary>
    [HttpPost("removeMembers")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(MemberView), StatusCodes.Status200OK)]
    public async Task<Result<List<MemberView>>> RemoveMembers([FromBody] RemoveMembersDto removeMembersDto)
    {
        var response = await _projectService.RemoveMembers(removeMembersDto);
        _logger.Log(removeMembersDto, response, HttpContext, "Удалены члены проекта.", Logging.Entities.TypeRequest.DELETE, Logging.Entities.ModuleAstrum.Project);
        return response;
    }
}