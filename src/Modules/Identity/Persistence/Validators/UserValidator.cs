using Astrum.Identity.Models;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Validation;
using IdentityModel;
using Microsoft.AspNetCore.Identity;

namespace Astrum.Identity.Validators;

public class UserValidator : IResourceOwnerPasswordValidator
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserValidator(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    #region IResourceOwnerPasswordValidator Members

    public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
    {
        var user = await _userManager.FindByNameAsync(context.UserName);
        user ??= await _userManager.FindByEmailAsync(context.UserName);
        if (user == null)
        {
            context.Result = new GrantValidationResult(
                TokenRequestErrors.UnauthorizedClient, "User not found");
            return;
        }

        var result = await _signInManager.PasswordSignInAsync(context.UserName, context.Password, true, true);

        if (result.Succeeded)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            // context set to success
            context.Result = new GrantValidationResult(
                user.Id.ToString(),
                OidcConstants.AuthenticationMethods.Password,
                claims
            );
            return;
        }

        // context set to Failure        
        context.Result = new GrantValidationResult(
            TokenRequestErrors.UnauthorizedClient, "Username or password is invalid");
    }

    #endregion
}