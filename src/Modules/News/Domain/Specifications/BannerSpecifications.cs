using Ardalis.Specification;
using Astrum.News.Aggregates;

namespace Astrum.News.Specifications;

public class GetBannersSpec : Specification<Banner>
{
}

public class GetBannerByIdSpec : GetBannersSpec
{
    public GetBannerByIdSpec(Guid id)
    {
        Query
            .Where(x => x.Id == id);
    }
}

public class GetActiveBannersSpec : GetBannersSpec
{
    public GetActiveBannersSpec()
    {
        Query
            .Where(banner => banner.IsActive);
    }
}