using Ardalis.Specification;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Example.Repositories;

/// <summary>
///     Implementation of <see cref="IExampleRepository" /> which allows persistence on both EventStore and relational
///     store.
/// </summary>
public class ExampleRepository : EFRepository<Aggregates.Example, Guid, ExampleDbContext>,
    IExampleRepository
{
    public ExampleRepository(ExampleDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(
        context, specificationEvaluator)
    {
    }
}