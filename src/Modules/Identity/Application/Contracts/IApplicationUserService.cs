using Astrum.Identity.DTOs;
using Astrum.Identity.Models;
using Astrum.SharedLib.Common.Options;
using Astrum.SharedLib.Common.Results;
using Result = Astrum.SharedLib.Common.Results.Result;

namespace Astrum.Identity.Contracts;

public interface IApplicationUserService
{
    /// <summary>
    ///     Creates a new user according to the provided <paramref name="user" />
    /// </summary>
    /// <param name="user"></param>
    /// <param name="password"></param>
    /// <param name="roles"></param>
    /// <param name="isActive"></param>
    /// <returns></returns>
    Task<Result> CreateUser(ApplicationUser user, string password, List<string>? roles, bool isActive);

    /// <summary>
    ///     Activates the application user with username <paramref name="username" />
    /// </summary>
    /// <param name="username"></param>
    /// <returns></returns>
    Task<Result> ActivateUser(string username);

    /// <summary>
    ///     Deactivates the application user with username <paramref name="username" />
    /// </summary>
    /// <returns></returns>
    Task<Result> DeactivateUser(string username);

    /// <summary>
    ///     Adds a role to a user
    /// </summary>
    /// <param name="request">the role assignment request</param>
    /// <returns></returns>
    Task<Result> AddRoles(RoleAssignmentRequestDto request);

    /// <summary>
    ///     Removes a role from a user
    /// </summary>
    /// <param name="request">the role assignment request</param>
    /// <returns></returns>
    Task<Result> RemoveRoles(RoleAssignmentRequestDto request);

    /// <summary>
    ///     Updates a user's roles
    /// </summary>
    /// <param name="username">the username</param>
    /// <param name="roles">the list of new roles</param>
    /// <returns></returns>
    Task<Result> UpdateRoles(string username, List<string> roles);

    /// <summary>
    ///     Retrieves all users
    /// </summary>
    /// <returns>list of <see cref="ApplicationUser" /></returns>
    Task<Result<List<ApplicationUser>>> GetAllUsersAsync(QueryOptions options = null,
        CancellationToken cancellationToken = default);

    /// <summary>
    ///     Get user
    /// </summary>
    /// <param name="id">the user ID</param>
    /// <param name="cancellationToken"></param>
    /// <returns> a <see cref="ApplicationUser" /></returns>
    Task<Result<ApplicationUser>> GetUserAsync(Guid id, CancellationToken cancellationToken = default);
    /// <summary>
    ///     Get user by username
    /// </summary>
    /// <param name="username"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    Task<Result<ApplicationUser>> GetUserByNameAsync(string username, CancellationToken cancellationToken = default);

    /// <summary>
    ///     Changes a user's password
    /// </summary>
    /// <param name="username"></param>
    /// <param name="password"></param>
    /// <returns></returns>
    Task<Result> ChangePassword(string username, string password);

    Task<Result> UpdateUserDetails(UpdateUserDetailsDto request);
}