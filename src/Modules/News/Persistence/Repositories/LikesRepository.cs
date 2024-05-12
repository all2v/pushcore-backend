using Ardalis.Specification;
using Astrum.News.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.News.Repositories;

/// <summary>
///     Implementation of <see cref="IExampleRepository" /> which allows persistence on both EventStore and relational
///     store.
/// </summary>
public class LikesRepository : EFRepository<Like, Guid, NewsDbContext>,
    ILikesRepository
{
    public LikesRepository(NewsDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(
        context, specificationEvaluator)
    {
    }
}