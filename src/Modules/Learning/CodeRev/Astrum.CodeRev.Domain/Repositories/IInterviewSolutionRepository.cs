using Astrum.CodeRev.Domain.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.CodeRev.UserService.DomainService.Repositories;

public interface IInterviewSolutionRepository : IEntityRepository<InterviewSolution, Guid>
{
    
}