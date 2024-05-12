using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Features.Profile.Queries;

public class GetUserProfileByUsernameQuery : QueryResult<UserProfileResponse>
{
    public GetUserProfileByUsernameQuery(string username)
    {
        Username = username;
    }

    public string Username { get; set; }
}