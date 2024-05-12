using Ardalis.Specification;
using Astrum.CodeRev.Domain.Aggregates;
using Astrum.CodeRev.UserService.DomainService.Repositories;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.CodeRev.Persistence.Repositories;

public class InterviewSolutionRepository : EFRepository<InterviewSolution, Guid, CodeRevDbContext>,
    IInterviewSolutionRepository
{
    public InterviewSolutionRepository(CodeRevDbContext context, ISpecificationEvaluator? specificationEvaluator = null)
        : base(context, specificationEvaluator)
    {
    }
}