using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.CodeRev.Domain.Repositories;
using Astrum.CodeRev.Domain.Specifications.InterviewLanguage;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.UserService.Services.Interviews;

public class InterviewLanguageService : IInterviewLanguageService
{
    private readonly IInterviewLanguageRepository _interviewLanguageRepository;

    public InterviewLanguageService(IInterviewLanguageRepository interviewLanguageRepository)
    {
        this._interviewLanguageRepository = interviewLanguageRepository;
    }

    private async Task<Dictionary<Guid, List<ProgrammingLanguage>>> GetInterviewsWithLanguages()
    {
        var languages = await _interviewLanguageRepository.ListAsync();
        return languages
            .GroupBy(interviewAndLanguage => interviewAndLanguage.Id,
                interviewAndLanguage => interviewAndLanguage.ProgrammingLanguage)
            .ToDictionary(interviewWithLanguage => interviewWithLanguage.Key,
                interviewWithLanguage => interviewWithLanguage.ToList());
    }

    public async Task<Result<List<ProgrammingLanguage>>> GetInterviewLanguages(Guid interviewId)
    {
        var lang = await _interviewLanguageRepository.ListAsync(new GetInterviewLanguagesByInterviewId(interviewId));
        return Result<List<ProgrammingLanguage>>.Success(lang
            .Select(interviewLanguage => interviewLanguage.ProgrammingLanguage)
            .ToList());
    }
}