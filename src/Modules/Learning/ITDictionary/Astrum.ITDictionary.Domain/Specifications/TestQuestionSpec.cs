using Ardalis.Specification;
using Astrum.ITDictionary.Aggregates;

namespace Astrum.ITDictionary.Specifications;

public class GetQuestionSpec: Specification<TestQuestion>
{
    public GetQuestionSpec()
    {
        Query
            .Include(e => e.AnswerOptions);
    }
}

public class GetQuestionWithIncludesSpec : GetQuestionSpec
{
    public GetQuestionWithIncludesSpec()
    {
        Query
            .Include(e => e.Practice)
            .Include(e => e.AnswerOptions)
            .ThenInclude(e => e.TermSource)
            .ThenInclude(e => e.Category);
    }
}

public class GetQuestionByIdSpec : GetQuestionSpec
{
    public GetQuestionByIdSpec(Guid id)
    {
        Query
            .Where(e => e.Id == id);
    }
}

public class GetQuestionsByPracticeIdSpec : GetQuestionSpec
{
    public GetQuestionsByPracticeIdSpec(Guid practiceId)
    {
        Query
            .Where(e => e.PracticeId == practiceId);
    }
}

public class GetQuestionByPracticeIdsSpec : GetQuestionSpec
{
    public GetQuestionByPracticeIdsSpec(ICollection<Guid> practices)
    {
        Query
            .Where(e => practices.Contains(e.PracticeId));
    }
}

public class GetQuestionByUserIdSpec : GetQuestionSpec
{
    public GetQuestionByUserIdSpec(Guid userId)
    {
        Query
            .Where(e => e.Practice.UserId == userId);
    }
}

public class GetQuestionByIdsSpec : GetQuestionByIdSpec
{
    public GetQuestionByIdsSpec(Guid userId, Guid id): base(id)
    {
        Query
            .Where(e => e.Practice.UserId == userId);
    }
}