using Astrum.Identity.Authorization.Operations;
using Astrum.Identity.Authorization.Requirements;
using Astrum.Identity.Extensions;
using Astrum.SharedLib.Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Astrum.Identity.Authorization.Handlers;

/// <summary>
///     Authorization handler for user operations. See <see cref="UserOperations" />
/// </summary>
public class UserOperationAuthorizationHandler : AuthorizationHandler<UserOperationAuthorizationRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
        UserOperationAuthorizationRequirement requirement)
    {
        switch (requirement.Name)
        {
            case OPERATIONS.USER.CREATE:
            case OPERATIONS.USER.EDIT:
            case OPERATIONS.USER.DELETE:
            case OPERATIONS.USER.READ:
                context.EvaluateRequirement(requirement, () => context.User.IsInRole(RolesEnum.Admin.ToString()));
                break;
        }

        return Task.CompletedTask;
    }
}