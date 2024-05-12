using System.Security.Claims;
using Astrum.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Astrum.Identity.Factories;

public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
{
    public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
        IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor)
    {
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        identity.AddClaim(new Claim(ApplicationUserClaims.FullNameClaimType, user.Name));
        identity.AddClaim(new Claim(ApplicationUserClaims.CultureClaimType, user.Culture));
        identity.AddClaim(new Claim(ApplicationUserClaims.UiCultureClaimType, user.UICulture));
        identity.AddClaim(new Claim(ApplicationUserClaims.ThemeClaimType, user.Theme.ToString()));

        if (user.ProfilePictureResourceId.HasValue)
            identity.AddClaim(new Claim(ApplicationUserClaims.ProfilePictureClaimType,
                user.ProfilePictureResourceId.Value.ToString()));

        if (user.MustChangePassword)
            identity.AddClaim(new Claim(ApplicationUserClaims.MustChangePasswordClaimType, string.Empty));

        var roles = await UserManager.GetRolesAsync(user);
        foreach (var role in roles)
            identity.AddClaim(new Claim(ClaimTypes.Role, role));

        return identity;
    }
}