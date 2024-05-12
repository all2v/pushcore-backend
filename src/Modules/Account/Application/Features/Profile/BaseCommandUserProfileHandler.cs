using Astrum.Account.Services;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.CQS.Interfaces;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Features.Profile;

public abstract class BaseCommandUserProfileHandler<TCommand> : CommandResultHandler<TCommand>
    where TCommand : class, ICommand<Result>
{
    protected readonly IUserProfileService _userProfileService;

    public BaseCommandUserProfileHandler(IUserProfileService userProfileService)
    {
        _userProfileService = userProfileService;
    }
}