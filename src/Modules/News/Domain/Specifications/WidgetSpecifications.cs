using Ardalis.Specification;
using Astrum.News.Aggregates;

namespace Astrum.News.Specifications;

public class GetWidgetsSpec : Specification<Widget>
{
}

public class GetWidgetByIdSpec : GetWidgetsSpec
{
    public GetWidgetByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}

public class GetActiveWidgetsSpec : GetWidgetsSpec
{
    public GetActiveWidgetsSpec()
    {
        Query
            .Where(banner => banner.IsActive);
    }
}