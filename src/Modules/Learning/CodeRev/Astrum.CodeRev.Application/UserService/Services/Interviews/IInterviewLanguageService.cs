using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.UserService.Services.Interviews;

public interface IInterviewLanguageService
{
    public Task<Result<List<ProgrammingLanguage>>> GetInterviewLanguages(Guid interviewId);
}