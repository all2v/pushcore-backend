using Astrum.Account.Features.Account.AccountDetails;
using Astrum.Identity.Authorization.Operations;
using Astrum.Identity.Contracts;
using Astrum.Identity.Features.Queries;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Astrum.Account.Features.Account.UserEdit.Queries;

public sealed class UserEditQueryHandler : QueryResultHandler<UserEditQuery, EditUserResponse>
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IAuthenticatedUserService _userService;

    public UserEditQueryHandler(
        IHttpContextAccessor contextAccessor,
        IAuthorizationService authorizationService,
        IMediator mediator,
        IAuthenticatedUserService userService,
        IMapper mapper
    )
    {
        _contextAccessor = contextAccessor;
        _authorizationService = authorizationService;
        _mediator = mediator;
        _userService = userService;
        _mapper = mapper;
    }

    public override async Task<Result<EditUserResponse>> Handle(UserEditQuery query,
        CancellationToken cancellationToken = default)
    {
        var authorizationResult =
            await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, null, UserOperations.Edit());
        if (!authorizationResult.Succeeded)
            return Result.Error("You are not authorized to edit a user");

        var userResult = await _mediator.Send(new GetUser {Id = query.Id}, cancellationToken);
        if (!userResult.IsSuccess || userResult.Data == null)
            return Result.Error("User not found");

        var userEditModel = _mapper.Map<EditUserResponse>(userResult.Data);
        return Result.Success(userEditModel);
    }
}