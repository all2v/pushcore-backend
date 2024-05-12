using Astrum.Identity.Authorization.Requirements;

namespace Astrum.Identity.Authorization.Operations;

/// <summary>
///     Holds <see cref="UserOperationAuthorizationRequirement" />s for user
/// </summary>
public static class UserOperations
{
    public static UserOperationAuthorizationRequirement Create = new() {Name = OPERATIONS.USER.CREATE};

    public static UserOperationAuthorizationRequirement Read()
    {
        return new UserOperationAuthorizationRequirement {Name = OPERATIONS.USER.READ};
    }

    public static UserOperationAuthorizationRequirement Edit()
    {
        return new UserOperationAuthorizationRequirement {Name = OPERATIONS.USER.EDIT};
    }

    public static UserOperationAuthorizationRequirement Delete()
    {
        return new UserOperationAuthorizationRequirement {Name = OPERATIONS.USER.DELETE};
    }
}