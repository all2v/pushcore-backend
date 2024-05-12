using Astrum.Identity.Models;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Astrum.Identity.Managers;

public class ApplicationSignInManager : SignInManager<ApplicationUser>
{
    public ApplicationSignInManager(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor,
        IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, IOptions<IdentityOptions> optionsAccessor,
        ILogger<ApplicationSignInManager> logger, IAuthenticationSchemeProvider schemes,
        IUserConfirmation<ApplicationUser> confirmation) : base(userManager, contextAccessor, claimsFactory,
        optionsAccessor, logger, schemes, confirmation)
    {
    }


    public override async Task<bool> CanSignInAsync(ApplicationUser user)
    {
        var defaultCanSignIn = await base.CanSignInAsync(user);
        var errors = new List<ValidationFailure>();
        if (!user.IsActive)
            errors.Add(new ValidationFailure(nameof(user), "User is not active."));
        if (!defaultCanSignIn || errors.Any())
            return false;
        return true;
    }
}