using Astrum.Account.Aggregates;
using Astrum.Account.Features.Profile.Commands;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Account.Services;

public interface ISocialNetworksService
{
    public Task<Result<SocialNetworks>> UpdateAsync(UserProfile profile, EditSocialNetworksCommand command);
}