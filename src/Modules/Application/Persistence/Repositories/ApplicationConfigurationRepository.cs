using Ardalis.Specification;
using Astrum.Application.Aggregates;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.Application.Repositories;

internal class ApplicationConfigurationRepository :
    EFRepository<ApplicationConfiguration, string, ApplicationDbContext>,
    IApplicationConfigurationRepository
{
    public ApplicationConfigurationRepository(ApplicationDbContext context,
        ISpecificationEvaluator? specificationEvaluator = null) : base(context, specificationEvaluator)
    {
    }
}