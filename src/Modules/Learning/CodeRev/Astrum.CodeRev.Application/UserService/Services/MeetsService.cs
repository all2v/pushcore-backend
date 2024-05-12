using Astrum.CodeRev.Application.UserService.Services.Interviews;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.SyncInterviews;
using Astrum.CodeRev.Domain.Specifications.Interviews;
using Astrum.CodeRev.UserService.DomainService.Repositories;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.UserService.Services;

public class MeetsService : IMeetsService
{
    private readonly IInterviewSolutionRepository _interviewSolutionRepository;
    private readonly IInterviewLanguageService _interviewLanguageService;

    public MeetsService(IInterviewLanguageService interviewLanguageService,
        IInterviewSolutionRepository interviewSolutionRepository)
    {
        _interviewLanguageService = interviewLanguageService;
        _interviewSolutionRepository = interviewSolutionRepository;
    }

    public async Task<Result<List<MeetInfoDto>>> GetMeets(Guid requestingUserId, int offset, int limit)
    {
        var meets = await _interviewSolutionRepository.ListAsync(new GetInterviewSolutionsByCondition(
            solution => !solution.IsSubmittedByCandidate && solution.IsSynchronous,
            offset,
            limit
        ));

        var meetsInfo = new List<MeetInfoDto>(meets.Count);
        foreach (var solution in meets)
        {
            var meet = new MeetInfoDto
            {
                UserId = solution.UserId,
                InterviewSolutionId = solution.Id,
                InterviewId = solution.Interview.Id,
                Vacancy = solution.Interview.Vacancy,
                ProgrammingLanguages = await _interviewLanguageService.GetInterviewLanguages(solution.Interview.Id),
                IsOwnerMeet = solution.InvitedBy.Equals(requestingUserId),
                FirstName = solution.FirstName,
                Surname = solution.Surname,
                TasksCount = solution.Interview.Tasks.Count
            };
            meetsInfo.Add(meet);
        }

        return Result<List<MeetInfoDto>>.Success(meetsInfo);
    }
}