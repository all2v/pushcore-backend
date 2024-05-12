using Ardalis.Specification;
using Astrum.Projects.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Projects.Repositories
{
    public class ProductRepository : EFRepository<Product,  Guid, ProjectDbContext>,
    IProductRepository
    {
        public ProductRepository(ProjectDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(context, specificationEvaluator)
        {
        }
    }
}
