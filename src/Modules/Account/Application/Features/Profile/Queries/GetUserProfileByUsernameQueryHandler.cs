using Astrum.Account.Services;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Astrum.Account.Features.Profile.Queries;

public class
    GetUserProfileByUsernameQueryHandler : QueryResultHandler<GetUserProfileByUsernameQuery, UserProfileResponse>
{
    private readonly IAuthorizationService _authorizationService;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly IUserProfileService _userProfileService;

    public GetUserProfileByUsernameQueryHandler(IUserProfileService userProfileService,
        IAuthorizationService authorizationService, IHttpContextAccessor contextAccessor)
    {
        _userProfileService = userProfileService;
        _authorizationService = authorizationService;
        _contextAccessor = contextAccessor;
    }

    public override async Task<Result<UserProfileResponse>> Handle(GetUserProfileByUsernameQuery query,
        CancellationToken cancellationToken = default)
    {
        //var authorizationResult =
        //await _authorizationService.AuthorizeAsync(_contextAccessor.HttpContext.User, null, UserOperations.Read());
        //if (!authorizationResult.IsSuccess )
        //    return Result.Error<UserProfileResponse>("You are not authorized to view users");
        var response = await _userProfileService.GetUserProfileByUsernameAsync(query.Username);
        return response;
    }
}