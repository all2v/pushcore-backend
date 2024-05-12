using Ardalis.Specification;
using Astrum.News.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.News.Repositories;

/// <summary>
///     Implementation of <see cref="IExampleRepository" /> which allows persistence on both EventStore and relational
///     store.
/// </summary>
public class BannersRepository : EFRepository<Banner, Guid, NewsDbContext>,
    IBannersRepository
{
    public BannersRepository(NewsDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(
        context, specificationEvaluator)
    {
    }
}