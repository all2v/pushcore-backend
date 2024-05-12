using Astrum.CodeRev.Application.UserService.ViewModel.DTO;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Users;
using Astrum.CodeRev.Domain.Aggregates;
using Astrum.CodeRev.Domain.Aggregates.Enums;
using Astrum.CodeRev.Domain.Repositories;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.UserService.Services;

public class InvitationService : IInvitationService
{
    private const long InvitationDurationMs = 604800000; // == 1 week //todo make config setting

    private readonly IInvitationRepository _invitationRepository;
    private readonly IInterviewRepository _interviewRepository;

    public InvitationService(IInvitationRepository invitationRepository, IInterviewRepository interviewRepository)
    {
        this._invitationRepository = invitationRepository;
        _interviewRepository = interviewRepository;
    }

    public async Task<Result<InvitationResponse>> Create(InvitationParams invitationParams, Guid creatorId)
    {
        if (!Enum.TryParse(invitationParams.Role, true, out Role roleEnum))
            return Result<InvitationResponse>.Error("role is invalid");

        var mustBeInterviewId = invitationParams.InterviewId != null || roleEnum == Role.Candidate;
        var interviewGuid = Guid.Empty;
        if (mustBeInterviewId)
            interviewGuid = GuidParser.TryParse(invitationParams.InterviewId);

        if (mustBeInterviewId && (await _interviewRepository
                .FirstOrDefaultAsync(interview => interview.Id == interviewGuid)) == null)
            return Result<InvitationResponse>.Error($"Не существует интервью с id {interviewGuid}");


        var invitation = await _invitationRepository.FirstOrDefaultAsync(i =>
            i.Role == roleEnum && i.InterviewId == interviewGuid &&
            i.IsSynchronous == invitationParams.IsSynchronous);

        if (invitation == null)
        {
            invitation = new Invitation
            {
                Id = Guid.NewGuid(),
                Role = roleEnum,
                InterviewId = interviewGuid,
                ExpiredAt = DateTimeOffset.Now.ToUnixTimeMilliseconds() + InvitationDurationMs,
                CreatedBy = creatorId.ToString(),
                IsSynchronous = invitationParams.IsSynchronous,
            };
            await _invitationRepository.AddAsync(invitation);
        }
        else
            invitation.ExpiredAt = DateTimeOffset.Now.ToUnixTimeMilliseconds() + InvitationDurationMs;

        await _invitationRepository.UnitOfWork.SaveChangesAsync();
        return Result<InvitationResponse>.Success(new InvitationResponse
        {
            Invitation = invitation.Id
        });
    }

    public async Task<Result<Invitation>> GetInvitationById(Guid invitationId)
    {
        var result = await _invitationRepository.FirstOrDefaultAsync(invitation => invitation.Id == invitationId);
        return result == null
            ? Result<Invitation>.Error($"Не существует приглашения с id {invitationId}")
            : Result<Invitation>.Success(result);
    }

    public async Task<Result> CheckForDeadline(string invitationId)
    {
        var invitationGuid = GuidParser.TryParse(invitationId);
        if (invitationGuid == Guid.Empty)
            return Result.Error("Неверный фромат GUID");

        var invitation = await _invitationRepository.FirstOrDefaultAsync(i => i.Id == invitationGuid);

        if (invitation == null) return Result.Error($"Приглашения с id {invitationGuid} не сущетсвует");

        if (invitation.ExpiredAt < DateTimeOffset.Now.ToUnixTimeMilliseconds())
        {
            //await _invitationRepository.DeleteAsync(invitation);
            //await _invitationRepository.UnitOfWork.SaveChangesAsync();
            return Result.Error($"Время приглашения с id {invitationGuid} истекло");
        }

        return Result.Success();
    }
}