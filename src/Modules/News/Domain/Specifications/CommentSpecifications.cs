using Ardalis.Specification;
using Astrum.News.Aggregates;

namespace Astrum.News.Specifications;

public class GetCommentsSpec : Specification<Comment>
{
}

public class GetCommentByIdSpec : GetCommentsSpec
{
    public GetCommentByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}