using Ardalis.Specification;
using Astrum.News.Aggregates;

namespace Astrum.News.Specifications;

public class GetLikesSpec : Specification<Like>
{
}

public class GetLikeByIdSpec : GetLikesSpec
{
    public GetLikeByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}