using Ardalis.Specification;
using Astrum.News.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.News.Repositories;

/// <summary>
///     Implementation of <see cref="IExampleRepository" /> which allows persistence on both EventStore and relational
///     store.
/// </summary>
public class CommentsRepository : EFRepository<Comment, Guid, NewsDbContext>,
    ICommentsRepository
{
    public CommentsRepository(NewsDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(
        context, specificationEvaluator)
    {
    }
}