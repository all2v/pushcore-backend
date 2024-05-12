using Ardalis.Specification;
using Astrum.CodeRev.Domain.Aggregates;
using Astrum.CodeRev.Domain.Repositories;
using Astrum.SharedLib.Persistence.Repositories;

namespace Astrum.CodeRev.Persistence.Repositories;

public class TaskSolutionRepository : EFRepository<TaskSolution, Guid, CodeRevDbContext>, ITaskSolutionRepository
{
    public TaskSolutionRepository(CodeRevDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(context, specificationEvaluator)
    {
    }
}