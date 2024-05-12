using Ardalis.Specification;
using Astrum.Inventory.Domain.Aggregates;

namespace Astrum.Inventory.Domain.Specifications
{
    public class GetTemplatesSpec : Specification<Template>
    {
    }
    public class GetTemplateById : GetTemplatesSpec
    {
        public GetTemplateById(Guid id)
        {
            Query
                .Where(c => c.Id == id);
        }
    }
}
