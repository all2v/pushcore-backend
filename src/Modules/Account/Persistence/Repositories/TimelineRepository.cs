using Ardalis.Specification;
using Astrum.Account.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Account.Repositories;

public class TimelineRepository : EFRepository<AccessTimeline, Guid, AccountDbContext>, ITimelineRepository
{
    public TimelineRepository(AccountDbContext context, ISpecificationEvaluator? specificationEvaluator = null) :
        base(context, specificationEvaluator)
    {
    }
}