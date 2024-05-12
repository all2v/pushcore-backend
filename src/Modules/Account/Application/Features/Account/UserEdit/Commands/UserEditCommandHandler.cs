using System.Transactions;
using Astrum.Account.Application.Features.Profile.Commands;
using Astrum.Account.Features.Account.AccountDetails;
using Astrum.Identity.Authorization.Operations;
using Astrum.Identity.Contracts;
using Astrum.Identity.Features.Commands;
using Astrum.Identity.Features.Queries;
using Astrum.Identity.ReadModels;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.SharedLib.Persistence.Helpers;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Astrum.Account.Features.Account.UserEdit.Commands;

public sealed class UserEditCommandHandler : CommandResultHandler<UserEditCommand, EditUserResponse>
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ILocalizationService _localizer;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IAuthenticatedUserService _userService;

    public UserEditCommandHandler(
        IHttpContextAccessor contextAccessor,
        IAuthorizationService authorizationService,
        IMediator mediator,
        IAuthenticatedUserService userService,
        IMapper mapper, ILocalizationService localizer
    )
    {
        _contextAccessor = contextAccessor;
        _authorizationService = authorizationService;
        _mediator = mediator;
        _userService = userService;
        _mapper = mapper;
        _localizer = localizer;
    }

    public override async Task<Result<EditUserResponse>> Handle(UserEditCommand command,
        CancellationToken cancellationToken = default)
    {
        //var authorizationResult =
        //    await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, null, UserOperations.Edit());
        //if (!authorizationResult.Succeeded)
        //    return Result.Error("You are not authorized to edit a user");

        var userResult = await _mediator.Send(new GetUser {Id = command.Id}, cancellationToken);
        if (!userResult.IsSuccess || userResult.Data == null)
            return Result.NotFound("Пользователь не найден.");

        if (await HandleUserUpdate(command, userResult.Data) == false)
            return Result.Error("Ошибка при обновлении пользователя.");
        var userEditModel = _mapper.Map<EditUserResponse>(userResult.Data);
        return Result.Success(userEditModel);
    }

    private async Task<bool> HandleUserUpdate(UserEditCommand model, UserReadModel user)
    {
        try
        {
            //using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
            //{
            var updateUserDetailsCommand = _mapper.Map<UpdateUserDetailsCommand>(model);
            var updateResult = await _mediator.Send(updateUserDetailsCommand);
            if (!updateResult.IsSuccess)
                throw new Exception(updateResult.Errors.ToString());

            var updateProfileCommand = _mapper.Map<EditProfileCommand>(model);
            var updateProfileResult = await _mediator.Send(updateProfileCommand);
            if (!updateProfileResult.IsSuccess)
                throw new Exception(updateProfileResult.Errors.ToString());

            if (!string.IsNullOrWhiteSpace(model.Password))
            {
                var changePasswordResult =
                    await _mediator.Send(new ChangeUserPasswordCommand(model.Password, model.Username));
                if (changePasswordResult.Failed)
                    throw new Exception(changePasswordResult.MessageWithErrors);
            }

            if (user.IsActive != model.IsActive)
            {
                var updateStatusResult = model.IsActive
                    ? await _mediator.Send(new ActivateUserCommand(model.Username))
                    : await _mediator.Send(new DeactivateUserCommand(model.Username));
                if (updateStatusResult.Failed)
                    throw new Exception(updateResult.MessageWithErrors);
            }

            if (!user.Roles.All(model.Roles.Contains) || user.Roles.Count() != model.Roles.Count())
            {
                var rolesUpdCommand = new UpdateUserRolesCommand(model.Username, RolesHelper.MapToStringRoles(model.Roles).ToList());
                var updateRolesResult = await _mediator.Send(rolesUpdCommand);
                if (updateRolesResult.Failed)
                    throw new Exception(updateRolesResult.MessageWithErrors);
            }

            //    transaction.Complete();
            //}
        }
        catch (Exception ex)
        {
            return false;
        }

        return true;
    }
}