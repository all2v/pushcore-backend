using Astrum.CodeRev.Domain.Aggregates;
using Astrum.SharedLib.Application.Contracts.Persistence.Repositories;

namespace Astrum.CodeRev.Domain.Repositories;

public interface IInterviewRepository : IEntityRepository<Interview, Guid>
{
    public Task<List<string>> GetDistinctVacanciesAsync(int offset,int limit);
}