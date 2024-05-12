using Astrum.Identity.Authorization.Operations;
using Astrum.Identity.Contracts;
using Astrum.Identity.Features.Queries;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Astrum.SharedLib.Domain.Enums;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Astrum.Account.Features.Account.AccountDetails;

public class AccountDetailsQuery : QueryResult<UserDetailsResultData>
{
    public AccountDetailsQuery(Guid? id)
    {
        Id = id;
    }

    public Guid? Id { get; set; }
}

public class UserDetailsResultData
{
    public Guid Id { get; set; }

    public string Username { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    public IEnumerable<RolesEnum> Roles { get; set; }

    public bool IsActive { get; set; }

    public IReadOnlyCollection<string> LocalizedRoles { get; set; }
}

// public class UserDetailsViewModel
// {
//     public Guid Id { get; set; }
//
//     [Display(Name = ResourceKeys.Labels_Username)]
//     public string Username { get; set; }
//
//     [Display(Name = ResourceKeys.Labels_Name)]
//     public string Name { get; set; }
//
//     [Display(Name = ResourceKeys.Labels_Email)]
//     public string Email { get; set; }
//
//     [Display(Name = ResourceKeys.Labels_Phone)]
//     public string PhoneNumber { get; set; }
//
//     [Display(Name = ResourceKeys.Labels_Roles)]
//     public IEnumerable<RolesEnum> Roles { get; set; }
//
//     [Display(Name = ResourceKeys.Labels_Status)]
//     public bool IsActive { get; set; }
//
//     public IReadOnlyCollection<string> LocalizedRoles { get; set; }
// }

public sealed class AccountDetailsQueryHandler : QueryResultHandler<AccountDetailsQuery, UserDetailsResultData>
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly IAuthenticatedUserService _userService;

    public AccountDetailsQueryHandler(
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

    public override async Task<Result<UserDetailsResultData>> Handle(AccountDetailsQuery query,
        CancellationToken cancellationToken = default)
    {
        var authorizationResult =
            await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, null, UserOperations.Read());
        if (!authorizationResult.Succeeded)
            return Result.Error("Вы не авторизованы, чтобы просмотреть пользователей.");
        var userId = query.Id ?? _userService.UserId;
        if (userId == null)
            return Result.Error("Пользователь не найден.");
        var userResult = await _mediator.Send(new GetUser {Id = userId.Value}, cancellationToken);
        if (!userResult.IsSuccess)
            return Result.Error("Не удалось извлечь пользователя.");
        var result = _mapper.Map<UserDetailsResultData>(userResult.Data);
        return Result.Success(result);
    }
}