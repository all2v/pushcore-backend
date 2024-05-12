using Ardalis.Specification;
using Astrum.ITDictionary.Aggregates;
using Astrum.ITDictionary.Enums;

namespace Astrum.ITDictionary.Specifications;

public class GetPracticeSpec: Specification<Practice>
{
    
}

public class GetPracticeByIdSpec : GetPracticeSpec
{
    public GetPracticeByIdSpec(Guid id)
    {
        Query
            .Where(p => p.Id == id);
    }
}

public class GetPracticesByTypeSpec : GetPracticeSpec
{
    public GetPracticesByTypeSpec(PracticeType type)
    {
        Query
            .Where(p => p.Type == type);
    }
}

public class GetPracticesByUserIdSpec : GetPracticeSpec
{
    public GetPracticesByUserIdSpec(Guid userId)
    {
        Query
            .Where(p => p.UserId == userId);
    }
}

public class GetFinishedPracticesSpec : GetPracticesByUserIdSpec
{
    public GetFinishedPracticesSpec(Guid userId): base(userId)
    {
        Query
            .Where(p => p.IsFinished);
    }
}

public class GetPracticesByIdsSpec : GetPracticeByIdSpec
{
    public GetPracticesByIdsSpec(Guid userId, Guid practiceId): base(practiceId)
    {
        Query
            .Where(p => p.UserId == userId);
    }
}