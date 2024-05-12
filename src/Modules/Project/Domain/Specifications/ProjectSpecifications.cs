using Ardalis.Specification;

namespace Astrum.Projects.Specifications.Customer;

public class GetProjectsSpec : Specification<Aggregates.Project>
{
    public GetProjectsSpec()
    {
        Query
            .Include(x => x.CustomFields)
            .Include(x => x.Members);
    }
}

public class GetProjectByIdSpec : GetProjectsSpec
{
    public GetProjectByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}

public class GetProjectsByMemberIdSpec : GetProjectsSpec
{
    public GetProjectsByMemberIdSpec(Guid memberId)
    {
        Query
            .Where(x => x.Members != null && x.Members.Any(m=>m.UserId == memberId));
    }
}