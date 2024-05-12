using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Features.Profile.Queries;

public class GetEditInfoByUsernameQuery : QueryResult<EditUserProfileResponse>
{
    public GetEditInfoByUsernameQuery(string username)
    {
        Username = username;
    }

    public string Username { get; set; }
}