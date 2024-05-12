using Ardalis.Specification;

namespace Astrum.Appeal.Specifications;

public class GetAppealSpec : Specification<Aggregates.Appeal>
{
    public GetAppealSpec()
    {
        Query
            .Include(x => x.AppealCategories)
            .ThenInclude(x => x.Category);
    }
}

public class GetAppealAsNoTrackingSpec : GetAppealSpec
{
    public GetAppealAsNoTrackingSpec()
    {
        Query
            .Include(x => x.AppealCategories).ThenInclude(x => x.Category)
            .AsNoTracking();
    }
}

public class GetAppealByIdSpec : GetAppealSpec
{
    public GetAppealByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}

public class GetAppealByIdAsNoTrackingSpec : GetAppealByIdSpec
{
    public GetAppealByIdAsNoTrackingSpec(Guid id) : base(id)
    {
        Query
            .AsNoTracking();
    }
}