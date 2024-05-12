using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Contest;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Interviews;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Review;
using Astrum.CodeRev.Domain.Aggregates;
using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.UserService.Services.Interviews;

public interface IInterviewService
{
    Task<Result<List<InterviewDto>>> GetAllInterviews(int offset,int limit);
    Task<Result<List<string>>> GetAllVacancies(int offset,int limit);
    Task<Result<InterviewCreationResponse>> CreateInterview(InterviewCreationDto interviewCreation, string creatorUsername);
    Task<Result> TryPutInterviewSolutionGrade(string interviewSolutionId, Grade grade);
    Task<Result> TryPutInterviewSolutionResult(string interviewSolutionId, InterviewResult interviewResult);
    Task<Result> TryPutInterviewSolutionComment(string interviewSolutionId, string reviewerComment);
    Task<Result> TryPutInterviewSolutionReview(InterviewSolutionReview interviewSolutionReview);
    Task<Result<Interview>> GetInterviewWithTasks(Guid interviewId);
    Task<InterviewSolution?> GetInterviewSolution(Guid interviewSolutionId);
    Task<Result<InterviewSolution>> GetInterviewSolution(string interviewSolutionId);
    Task<Result<InterviewSolutionDto>> GetInterviewSolutionInfo(string token, string interviewSolutionId);
    Task<Result> StartInterviewSolution(string interviewSolutionId);
    Task<Result> EndInterviewSolution(string interviewSolutionId);
    Task<Result<InterviewSolutionInfo>> GetInterviewSolutionInfo(Guid userId);
}