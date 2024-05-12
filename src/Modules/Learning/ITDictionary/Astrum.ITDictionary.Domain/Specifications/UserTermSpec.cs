using System.Linq.Expressions;
using Ardalis.Specification;
using Astrum.ITDictionary.Aggregates;

namespace Astrum.ITDictionary.Specifications;

public class GetUserTermSpec : Specification<UserTerm>
{
    public GetUserTermSpec()
    {
    }
}

public class GetUserTermWithIncludesSpec : GetUserTermSpec
{
    public GetUserTermWithIncludesSpec()
    {
        Query
            .Include(e => e.Term)
            .ThenInclude(e => e.Category);
    }
}

public class GetUserTermByUserIdSpec : GetUserTermWithIncludesSpec
{
    public GetUserTermByUserIdSpec(Guid userId)
    {
        Query
            .Where(e => e.UserId == userId);
    }
}

public class GetSelectedUserTermsSpec : GetUserTermByUserIdSpec
{
    public GetSelectedUserTermsSpec(Guid userId): base(userId)
    {
        Query
            .Where(e => e.IsSelected);
    }
}