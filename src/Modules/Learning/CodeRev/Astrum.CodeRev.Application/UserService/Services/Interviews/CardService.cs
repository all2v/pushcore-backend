using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Review;
using Astrum.CodeRev.Domain.Specifications.Interviews;
using Astrum.CodeRev.UserService.DomainService.Repositories;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.UserService.Services.Interviews;

public class CardService : ICardService
{
    private readonly IInterviewSolutionRepository _interviewSolutionRepository;
    private readonly IStatusCheckerService _statusCheckerService;
    private readonly IInterviewLanguageService _interviewLanguageService;


    public CardService(IStatusCheckerService statusCheckerService,
        IInterviewLanguageService interviewLanguageService,
        IInterviewSolutionRepository interviewSolutionRepository)
    {
        this._statusCheckerService = statusCheckerService;
        this._interviewLanguageService = interviewLanguageService;
        _interviewSolutionRepository = interviewSolutionRepository;
    }

    public async Task<Result<List<CardInfo>>> GetCards(int offset, int limit)
    {
        var interviewSolutions =
            await _interviewSolutionRepository.ListAsync(new GetPaginatedInterviewSolutions(offset,limit));

        var cardsInfo = new List<CardInfo>();
        foreach (var interviewSolution in interviewSolutions)
        {
            var card = new CardInfo
            {
                Username = interviewSolution.Username,
                FirstName = interviewSolution.FirstName,
                Surname = interviewSolution.Surname,
                InterviewSolutionId = interviewSolution.Id,
                Vacancy = interviewSolution.Interview.Vacancy,
                StartTimeMs = interviewSolution.StartTimeMs,
                EndTimeMs = interviewSolution.EndTimeMs,
                TimeToCheckMs = interviewSolution.TimeToCheckMs,
                ReviewerComment = interviewSolution.ReviewerComment,
                AverageGrade = interviewSolution.AverageGrade,
                InterviewResult = interviewSolution.InterviewResult,
                IsSubmittedByCandidate = interviewSolution.IsSubmittedByCandidate,
                IsSolutionTimeExpired = _statusCheckerService.IsSolutionTimeExpired(interviewSolution.EndTimeMs),
                HasReviewerCheckResult = _statusCheckerService.HasReviewerCheckResult(interviewSolution.AverageGrade),
                HasHrCheckResult = _statusCheckerService.HasHrCheckResult(interviewSolution.InterviewResult),
                ProgrammingLanguages =
                    (await _interviewLanguageService.GetInterviewLanguages(interviewSolution.Interview.Id)).Data,
                IsSynchronous = interviewSolution.IsSynchronous,
                DoneTasksCount = interviewSolution.TaskSolutions.Count(solution => solution.IsDone),
                TasksCount = interviewSolution.TaskSolutions.Count(),
            };
            cardsInfo.Add(card);
        }

        return Result<List<CardInfo>>.Success(cardsInfo);
    }
}