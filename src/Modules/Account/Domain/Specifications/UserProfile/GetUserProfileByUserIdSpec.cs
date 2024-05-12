using Ardalis.Specification;

namespace Astrum.Account.Specifications.UserProfile;

public class GetUserProfileWithIncludesSpec : Specification<Aggregates.UserProfile>
{
    public GetUserProfileWithIncludesSpec()
    {
        Query
            .Include(x => x.SocialNetworks)
            .Include(x => x.Timelines).ThenInclude(x => x.Intervals)
            .Include(x => x.CustomFields)
            .Include(x => x.Achievements)
            .Include(x => x.Position);
    }
}

public class GetUserProfileByUserIdSpec : GetUserProfileWithIncludesSpec
{
    public GetUserProfileByUserIdSpec(Guid userId)
    {
        Query
            .Where(x => x.UserId == userId);
    }
}

public class GetUserProfileByFilterSpec : GetUserProfileWithIncludesSpec
{
    public GetUserProfileByFilterSpec(Guid? positionId = null, string name = null)
    {
        Query
            .Where(x => positionId == null || x.PositionId == positionId)
            .Where(x => name == null || x.Name.ToLower().Contains(name.ToLower()));
    }
}

public class GetUsersProfilesSpec : GetUserProfileWithIncludesSpec
{
    public GetUsersProfilesSpec(string name = null, List<Guid>? positionIds = null)
    {
        if (name != null)
            Query
                .Where(profile => (profile.Name + " " + profile.Surname).ToLower().Trim().Contains(name.ToLower().Trim()) 
                || (profile.Surname + " " + profile.Name).ToLower().Trim().Contains(name.ToLower().Trim()));
        if (positionIds != null && positionIds.Any())
            Query
                .Where(profile => profile.PositionId.HasValue && positionIds.Contains(profile.PositionId.Value));
    }
}