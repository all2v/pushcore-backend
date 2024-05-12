using Astrum.Account.Application.Features.Registration.Commands.RejectRegistrationApplication;
using Astrum.Account.Features;
using Astrum.Account.Features.Registration;
using Astrum.Account.Features.Registration.Commands.RegistrationApplicationCreate;
using Astrum.Account.Features.Registration.Commands.RegistrationApplicationDelete;
using Astrum.Account.Features.Registration.Commands.RegistrationApplicationUpdate;
using Astrum.Account.Features.Registration.Commands.RegistrationApplicationUpdateStatus;
using Astrum.Account.Features.Registration.Queries.GetAllRegistrationApplications;
using Astrum.Account.Features.Registration.Queries.GetRegistrationApplication;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.Market.Services;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Astrum.Account.Controllers;

[Route("[controller]")]
public class RegistrationController : ApiBaseController
{
    /// <summary>
    ///     Получить заявку на регистрацию
    /// </summary>
    /// <param name="applicationId">Application identifier</param>
    /// <returns></returns>
    [HttpGet("{applicationId}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(RegistrationApplicationResponse), StatusCodes.Status200OK)]
    public async Task<Result<RegistrationApplicationResponse>> Get([FromRoute] Guid applicationId)
    {
        var request = new GetRegistrationApplicationQuery(applicationId);
        return await Mediator.Send(request);
    }

    /// <summary>
    ///     Список заявок на регистрацию
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(List<RegistrationApplicationResponse>), StatusCodes.Status200OK)]
    public async Task<Result<List<RegistrationApplicationResponse>>> GetAll()
    {
        var request = new GetRegistrationApplicationsListQuery();
        return await Mediator.Send(request);
    }

    /// <summary>
    ///     Создать заявку на регистрацию
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    [AllowAnonymous]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(RegistrationApplicationResponse), StatusCodes.Status200OK)]
    public async Task<Result<RegistrationApplicationResponse>> Create(
        [FromBody] RegistrationApplicationCreateCommand request)
    {
        return await Mediator.Send(request);
    }

    /// <summary>
    ///     Обновить заявку на регистрацию
    /// </summary>
    /// <param name="applicationId"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{applicationId}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(RegistrationApplicationResponse), StatusCodes.Status200OK)]
    public async Task<Result<RegistrationApplicationResponse>> Update([FromRoute] Guid applicationId,
        [FromBody] RegistrationApplicationUpdateRequestBody request)
    {
        var command = Mapper.Map<RegistrationApplicationUpdateCommand>(request);
        command.Id = applicationId;
        return await Mediator.Send(command);
    }

    /// <summary>
    ///     Одобрить заявку на регистрацию
    /// </summary>
    /// <returns></returns>
    [HttpPut("accept")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(RegistrationApplicationResponse), StatusCodes.Status200OK)]
    public async Task<Result<RegistrationApplicationResponse>> Approve(
        [FromQuery] ApproveRegistrationApplicationCommand command)
    {
        var registrationResult = await Mediator.Send(command);
        return registrationResult;
    }

    /// <summary>
    ///     Отклонить заявку на регистрацию
    /// </summary>
    /// <returns></returns>
    [HttpPut("reject")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(RegistrationApplicationResponse), StatusCodes.Status200OK)]
    public async Task<Result<RegistrationApplicationResponse>> Reject(
        [FromQuery] RejectRegistrationApplicationCommand command)
    {
        var registrationResult = await Mediator.Send(command);
        return registrationResult;
    }

    /// <summary>
    ///     Удалить заявку на регистрацию
    /// </summary>
    /// <param name="applicationId"></param>
    /// <returns></returns>
    [HttpDelete("{applicationId}")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(RegistrationApplicationResponse), StatusCodes.Status200OK)]
    public async Task<Result<RegistrationApplicationResponse>> Delete([FromRoute] Guid applicationId)
    {
        var request = new RegistrationApplicationDeleteCommand(applicationId);
        return await Mediator.Send(request);
    }
}