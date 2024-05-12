using Astrum.CodeRev.Application.UserService.ViewModel.DTO;
using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Users;
using Astrum.CodeRev.Domain.Aggregates;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.UserService.Services;

public interface IInvitationService
{
    Task<Result<InvitationResponse>> Create(InvitationParams invitationParams, Guid creatorId);
    Task<Result<Invitation>> GetInvitationById(Guid invitationId);
    Task<Result> CheckForDeadline(string invitationId);
}