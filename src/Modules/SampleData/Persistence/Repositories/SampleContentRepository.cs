using Ardalis.Specification;
using Astrum.SampleData.Aggregates;
using Astrum.SampleData.Persistence;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.SampleData.Repositories;

public class SampleContentRepository : EFRepository<SampleContentFile, Guid, SampleContentDbContext>, ISampleContentRepository
{
    public SampleContentRepository(SampleContentDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(
        context, specificationEvaluator)
    {
    }
}