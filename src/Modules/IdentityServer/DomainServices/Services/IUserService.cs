using Astrum.IdentityServer.Domain.ViewModels;
using Astrum.IdentityServer.DomainServices.Features.Commands;
using Astrum.IdentityServer.DomainServices.ViewModels;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Astrum.IdentityServer.DomainServices.Services;

public interface IUserService
{
    /// <summary>
    /// Create a new user.
    /// </summary>
    /// <remarks>
    /// Username must be unique.
    /// </remarks>
    /// <param name="user">User representation.</param>
    /// <returns></returns>
    Task<UserViewModel> CreateUser(UserCreateCommand user);
    Task<UserViewModel> GetUser(string userId);
    Task<UserViewModel> GetUserByUsername(string username);
    Task<IEnumerable<UserViewModel>> GetUsers(GetUsersRequestParameters parameters = null);
    //Task<UserViewModel> EditUser(UserEditCommand user);
    Task<UserTokenResult> GetBearerTokenByPassword(LoginByPasswordCommand loginRequest);
    Task<UserTokenResult> GetBearerTokenFromAuthCode(LoginCommand loginRequest);
    Task<TokenOperationResult> RefreshBearerToken(string inputRefreshToken);

    /// <summary>
    /// Send an email-verification email to the user.
    /// </summary>
    /// <remarks>
    /// An email contains a link the user can click to verify their email address.
    /// </remarks>
    /// <returns></returns>
    Task SendVerifyEmail();
}