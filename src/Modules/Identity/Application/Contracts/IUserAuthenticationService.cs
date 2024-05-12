using Astrum.Identity.Application.ViewModels;
using Astrum.Identity.DTOs;
using Astrum.SharedLib.Common.Results;

namespace Astrum.Identity.Contracts;

public interface IUserAuthenticationService
{
    Task<UserTokenResult> Login(LoginRequestDTO loginRequest, CancellationToken cancellationToken);
    Task<Result> Logout(CancellationToken cancellationToken);
    Task<TokenOperationResult> LoginGitlabUser(GitLabUserForm gitLabUserForm, CancellationToken cancellationToken = default);
    Task<GitlabUserCreateResult> GetOrCreateUserByGitlabId(GitLabUserForm gitLabUserForm);
}