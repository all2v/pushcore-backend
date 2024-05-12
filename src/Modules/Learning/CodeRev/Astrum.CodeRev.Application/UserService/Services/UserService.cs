using Astrum.CodeRev.Application.UserService.Services.Interviews;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Auth;
using Astrum.CodeRev.Domain.Aggregates;
using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.CodeRev.UserService.DomainService.Repositories;
using Astrum.SharedLib.Application.Helpers;
using Astrum.SharedLib.Common.Results;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Astrum.CodeRev.Application.UserService.Services;

public class UserService : IUserService
{
    //  private readonly HttpClient _client = new HttpClient();
    private readonly IInterviewSolutionRepository _interviewSolutionRepository;
    private readonly IInvitationService _invitationService;
    private const string RequestUrl = "http://localhost:50000/api/account/user-profile/basicinfo/";
    private readonly IInterviewService _interviewService;
    private readonly IDraftService _draftService;

    public UserService(IInterviewSolutionRepository interviewSolutionRepository, IInterviewService interviewService,
        IDraftService draftService, IInvitationService invitationService)
    {
        _interviewSolutionRepository = interviewSolutionRepository;
        _interviewService = interviewService;
        _draftService = draftService;
        _invitationService = invitationService;
    }

    public async Task<Result<Guid>> CreateInterviewSolution(string invitedUserToken, UserInvite userInvite,
        Guid invitationId)
    {
        var tokenClaims = JwtManager.GetJwtTokenClaims(invitedUserToken);

        var interviewSolutionGuid = Guid.NewGuid();
        var reviewerDraft = await _draftService.Create(interviewSolutionGuid);
        var invitation = await _invitationService.GetInvitationById(invitationId);
        if (!invitation.IsSuccess)
            return Result<Guid>.Error(invitation.MessageWithErrors);
        var interview = await _interviewService.GetInterviewWithTasks(invitation.Data.InterviewId);

        if (!interview.IsSuccess)
            return Result<Guid>.Error(interview.MessageWithErrors);
        var solution = new InterviewSolution
        {
            Id = interviewSolutionGuid,
            UserId = Guid.Parse(tokenClaims.First(
                claim => claim.Type == JwtRegisteredClaimNames.Sub).Value),
            Surname = userInvite.Surname,
            FirstName = userInvite.FirstName,
            Username = userInvite.Username,
            Interview = interview,
            Email = userInvite.Email,
            PhoneNumber = userInvite.PhoneNumber,
            ReviewerDraftId = reviewerDraft.Data.Id,
            ReviewerDraft = reviewerDraft,
            StartTimeMs = -1,
            EndTimeMs = DateTimeOffset.Now.ToUnixTimeMilliseconds() + interview.Data.InterviewDurationMs,
            TimeToCheckMs = -1,
            ReviewerComment = "",
            InterviewResult = InterviewResult.NotChecked,
            IsSubmittedByCandidate = false,
            InvitedBy = Guid.Parse(invitation.Data.CreatedBy),
            IsSynchronous = invitation.Data.IsSynchronous
        };

        solution.TaskSolutions = interview.Data.Tasks.Select(task => new TaskSolution
            {
                Id = Guid.NewGuid(),
                InterviewSolutionId = interviewSolutionGuid,
                InterviewSolution = solution,
                TaskId = task.Id,
                IsDone = false,
                Grade = Grade.Zero,
                RunAttemptsLeft = task.RunAttempts,
                TestTask = task
            })
            .ToList();
        await _interviewSolutionRepository.AddAsync(solution);
        await _interviewSolutionRepository.UnitOfWork.SaveChangesAsync();

        return Result<Guid>.Success(interviewSolutionGuid);
    }
}