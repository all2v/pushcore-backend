using Astrum.CodeRev.Application.UserService.ViewModel.DTO.Auth;
using Astrum.SharedLib.Common.Results;

namespace Astrum.CodeRev.Application.UserService.Services;

public interface IUserService
{
    //Task<FullUserName> GetFullUserNameInfo(string authToken, string username);
    //Task<string> GetFullNameByInterviewSolutionId(string authToken, Guid interviewSolutionId);
    Task<Result<Guid>> CreateInterviewSolution(string invitedUserToken, UserInvite userInvite, Guid invitationId);
}