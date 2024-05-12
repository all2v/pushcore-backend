using System.Linq.Dynamic.Core;
using Ardalis.Specification;
using Astrum.CodeRev.Domain.Aggregates;
using Astrum.CodeRev.Domain.Repositories;
using Astrum.SharedLib.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Astrum.CodeRev.Persistence.Repositories;

public class InterviewRepository : EFRepository<Interview, Guid, CodeRevDbContext>,
    IInterviewRepository
{
    public InterviewRepository(CodeRevDbContext context, ISpecificationEvaluator? specificationEvaluator = null) : base(
        context, specificationEvaluator)
    {
    }

    public async Task<List<string>> GetDistinctVacanciesAsync(int offset, int limit)
    {
        return await Items.Select(interview => interview.Vacancy)
            .Distinct()
            .Skip(offset)
            .Take(limit)
            .ToListAsync();
    }
}