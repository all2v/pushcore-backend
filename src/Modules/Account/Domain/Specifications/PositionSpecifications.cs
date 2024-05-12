using Ardalis.Specification;
using Astrum.Account.Domain.Aggregates;

namespace Astrum.Account.Specifications
{
    public class GetPositionsSpec : Specification<Position>
    {
    }

    public class GetPositionByIdSpec : GetPositionsSpec
    {
        public GetPositionByIdSpec(Guid id)
        {
            Query
                .Where(x => x.Id == id);
        }
    }

    public class GetPositionByNameSpec : GetPositionsSpec
    {
        public GetPositionByNameSpec(string name)
        {
            Query
                .Where(x => x.Name.ToLower() == name.ToLower());
        }
    }
}
