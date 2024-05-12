using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Astrum.Identity.Authorization;

/// <summary>
///     Custom authorization policy provider.
///     Falls back to the <see cref="DefaultAuthorizationPolicyProvider" /> for default and fallback policies
/// </summary>
public class CustomAuthorizationPolicyProvider : IAuthorizationPolicyProvider
{
    public CustomAuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
    {
        BackupPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
    }

    private DefaultAuthorizationPolicyProvider BackupPolicyProvider { get; }

    #region IAuthorizationPolicyProvider Members

    public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
    {
        return BackupPolicyProvider.GetDefaultPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
    {
        return BackupPolicyProvider.GetFallbackPolicyAsync();
    }

    public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        return BackupPolicyProvider.GetPolicyAsync(policyName);
    }

    #endregion

    private AuthorizationPolicy BuildAuthorizationPolicyFromRequirements(
        params IAuthorizationRequirement[] requirements)
    {
        var policy = new AuthorizationPolicyBuilder(CookieAuthenticationDefaults.AuthenticationScheme);
        policy.AddRequirements(requirements);
        return policy.Build();
    }
}