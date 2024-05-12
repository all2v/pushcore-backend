using Astrum.CodeRev.Domain.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.CodeRev.Domain.Repositories;

public interface ITaskSolutionRepository : IEntityRepository<TaskSolution, Guid>
{
    
}