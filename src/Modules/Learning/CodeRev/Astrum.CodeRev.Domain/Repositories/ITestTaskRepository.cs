using Astrum.CodeRev.Domain.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.CodeRev.Domain.Repositories;

public interface ITestTaskRepository : IEntityRepository<TestTask, Guid>
{
    
}