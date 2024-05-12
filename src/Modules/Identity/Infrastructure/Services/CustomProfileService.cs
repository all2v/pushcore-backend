using System.Security.Claims;
using Astrum.Identity.Models;
using Duende.IdentityServer.AspNetIdentity;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using IdentityModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Astrum.Identity.Services;

public class CustomProfileService : ProfileService<ApplicationUser>
{
    public CustomProfileService(UserManager<ApplicationUser> userManager,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, ILogger<CustomProfileService> logger) :
        base(userManager, claimsFactory, logger)
    {
    }


    protected override async Task GetProfileDataAsync(ProfileDataRequestContext context, ApplicationUser user)
    {
        // TODO rewrite others GetProfileDataAsync methods for adding logs
        // var sub = context.Subject?.GetSubjectId();
        // if (sub == null) throw new Exception("No sub claim present");
        //
        //     Logger.LogWarning("No user found matching subject Id: {0}", sub);
        var principal = await GetUserClaimsAsync(user);
        var id = (ClaimsIdentity)principal.Identity!;
        if (!string.IsNullOrEmpty(user.Culture)) id.AddClaim(new Claim("culture", user.Culture));
        var userRoles = await UserManager.GetRolesAsync(user);
        foreach (var userRole in userRoles)
            id.AddClaim(new Claim(JwtClaimTypes.Role, userRole));

        context.AddRequestedClaims(principal.Claims);
    }

    public override async Task IsActiveAsync(IsActiveContext context)
    {
        var sub = context.Subject?.GetSubjectId();
        if (sub == null) throw new Exception("No subject Id claim present");

        var user = await UserManager.FindByIdAsync(sub);
        if (user == null) Logger.LogWarning("No user found matching subject Id: {0}", sub);

        context.IsActive = user?.IsActive ?? false;
    }
}