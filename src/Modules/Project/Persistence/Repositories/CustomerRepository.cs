using Ardalis.Specification;
using Astrum.Projects.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Projects.Repositories;

public class CustomerRepository : EFRepository<Customer, Guid, ProjectDbContext>,
    ICustomerRepository
{
    public CustomerRepository(ProjectDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(
        context, specificationEvaluator)
    {
    }
}