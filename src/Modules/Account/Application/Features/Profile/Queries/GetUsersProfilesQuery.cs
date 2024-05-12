using Astrum.SharedLib.Common.CQS.Implementations;

namespace Astrum.Account.Features.Profile.Queries;

public class GetUsersProfilesQuery : QueryResult<List<UserProfileSummary>>
{
    public string Name { get; set; }
    public List<Guid>? PositionIds { get; set; }

    public GetUsersProfilesQuery(string name, List<Guid>? positionIds)
    {
        Name = name;
        PositionIds = positionIds;
    }
}