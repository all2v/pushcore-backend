using System.Data;
using Astrum.Identity.Application.Contracts;
using Astrum.Identity.Application.ViewModels;
using Astrum.Identity.Contracts;
using Astrum.Identity.Domain.Entities;
using Astrum.Identity.DTOs;
using Astrum.Identity.Infrastructure.Services;
using Astrum.Identity.Models;
using Astrum.SharedLib.Common.Results;
using Astrum.SharedLib.Persistence.Helpers;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using static MassTransit.ValidationResultExtensions;
using Result = Astrum.SharedLib.Common.Results.Result;

namespace Astrum.Identity.Services;

public class UserAuthenticationService : IUserAuthenticationService
{
    private readonly IUserConfirmation<ApplicationUser> _confirmation;
    private readonly IIdentityServerInteractionService _interaction;
    private readonly ILogger<UserAuthenticationService> _logger;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IIdentityJwtGenerator _jwtGenerator;
    private readonly IGitlabMappingService _gitlabMappingService;

    public UserAuthenticationService(ILogger<UserAuthenticationService> logger, IIdentityJwtGenerator jwtGenerator,
        SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager,
        /*IIdentityServerInteractionService interaction,*/ IUserConfirmation<ApplicationUser> confirmation, 
        IGitlabMappingService gitlabMappingService)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        //_interaction = interaction;
        _confirmation = confirmation;
        _logger = logger;
        _jwtGenerator = jwtGenerator;
        _gitlabMappingService = gitlabMappingService;
    }

    #region IUserAuthenticationService Members

    public async Task<UserTokenResult> Login(LoginRequestDTO loginRequest, CancellationToken cancellationToken)
    {
        //var context = await _interaction.GetAuthorizationContextAsync(loginRequest.ReturnUrl);

        //async Task OnCancelLoginRequest()
        //{
        //    if (context != null)
        //        // if the user cancels, send a result back into IdentityServer as if they 
        //        // denied the consent (even if this client does not require consent).
        //        // this will send back an access denied OIDC error response to the client.
        //        await _interaction.DenyAuthorizationAsync(context, AuthorizationError.AccessDenied);
        //    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
        //    // if (context.IsNativeClient())
        //    // The client is native, so this change in how to
        //    // return the response is for better UX for the end user.
        //    // return this.LoadingPage(Input.ReturnUrl);
        //    // return Task.FromCanceled<Result<string>>(cancellationToken); 
        //    // return Result.Success(loginRequest.ReturnUrl);
        //}

        //cancellationToken.Register(async () => await OnCancelLoginRequest());

        var result = new UserTokenResult();
        try
        {
            var user = await _userManager.FindByNameAsync(loginRequest.Login);
            user ??= await _userManager.FindByEmailAsync(loginRequest.Login);
            if (user == null)
            {
                result.ErrorMessage = $"Пользователь {loginRequest.Login} не найден.";
                return result;
            }

           
            var signInResult =  await _signInManager.CheckPasswordSignInAsync(user, loginRequest.Password, true);
               
            if (!signInResult.Succeeded)
            {
                result.ErrorMessage = "Неверный пароль.";
                return result;
            }

            //var  props =  new AuthenticationProperties();
            //if (AccountOptions.AllowRememberLogin && loginRequest.RememberMe)
            //{
            //    props.IsPersistent = true;
            //    props.ExpiresUtc = DateTimeOffset.UtcNow.Add(AccountOptions.RememberMeLoginDuration);
            //};

            //await _signInManager.SignInAsync(user, props);

            //var client = new HttpClient();
            //var disco = await client.GetDiscoveryDocumentAsync("https://localhost:7136", cancellationToken: cancellationToken);
            //if (disco.IsError)
            //    return Result.Error(disco.Error);

            //var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            //{
            //    Address = disco.TokenEndpoint,

            //    ClientId = "react_site",
            //    ClientSecret = "react_site",
            //    Scope = "astrum.api"
            //}, cancellationToken);
            // TODO return jwt
            var roles = await _userManager.GetRolesAsync(user);
            result.AccessToken = _jwtGenerator.CreateToken(user, roles);
            result.Successful = true;
            return result;
            //return Result.Success();
        }
        catch (Exception ex)
        {
            var errorMessage = "Ошибка при входе пользователя.";
            _logger.LogError(ex, errorMessage);
            result.ErrorMessage = $"{ex}:::{errorMessage}";
            result.Successful = false;
            result.AccessToken = null;
            return result;
        }
    }

    public async Task<Result> Logout(CancellationToken cancellationToken)
    {
        try
        {
            await _signInManager.SignOutAsync();
            return Result.SuccessWithMessage("Successfully logged out.");
        }
        catch (Exception ex)
        {
            var errorMessage = "Error while trying to sign out user.";
            _logger.LogError(ex, errorMessage);
            return Result.Error(errorMessage);
        }
    }

    public async Task<TokenOperationResult> LoginGitlabUser(GitLabUserForm gitLabUserForm, CancellationToken cancellationToken = default)
    {
        var result = new TokenOperationResult();
        var createUserResult = await GetOrCreateUserByGitlabId(gitLabUserForm);
        if (!createUserResult.Successful || createUserResult.User == null)
        {
            result.ErrorMessage = createUserResult.ErrorMessage;
            return result;
        }
        var token = _jwtGenerator.CreateToken(createUserResult.User);
        result.AccessToken = token;
        result.Successful = true;
        return result;
    }

    public async Task<GitlabUserCreateResult> GetOrCreateUserByGitlabId(GitLabUserForm gitLabUserForm)
    {
        var result = new GitlabUserCreateResult();
        if (!long.TryParse(gitLabUserForm.Id, out var longGitlabId))
        {
            result.ErrorMessage = "Cant parse gitlab user id!";
            return result;
        }
        var existingUser = await _gitlabMappingService.GetApplicationUserByGitlabId(longGitlabId);
        if (existingUser != null)
        {
            result.User = existingUser;
            result.Successful = true;
            return result;
        }
        else
        {
            var newGitlabUser = new GitlabUser
            {
                Email = gitLabUserForm.Email,
                Id = longGitlabId,
                Name = gitLabUserForm.Name,
                Username = gitLabUserForm.Username
            };
            var newUserId = await _gitlabMappingService.AddUserFromGitLab(newGitlabUser);
            result.User = await _userManager.FindByIdAsync(newUserId.ToString());
            if (result.User == null)
            {
                result.ErrorMessage = "Unable to find created user!";
                return result;
            }
            result.Successful = true;
            return result;
        }
    }

    #endregion

    private async Task<Result<string>> ValidateUserForLogin(ApplicationUser? user)
    {
        if (user == null)
            return Result.Error("User not found.");

        if (!user.IsActive)
            return Result.Error("User  is not active.");

        if (_signInManager.Options.SignIn.RequireConfirmedEmail && !await _userManager.IsEmailConfirmedAsync(user))
            return Result.Error("User cannot sign in without a confirmed email.");
        if (_signInManager.Options.SignIn.RequireConfirmedPhoneNumber &&
            !await _userManager.IsPhoneNumberConfirmedAsync(user))
            return Result.Error("User cannot sign in without a confirmed phone number.");

        if (_signInManager.Options.SignIn.RequireConfirmedAccount &&
            !await _confirmation.IsConfirmedAsync(_userManager, user))
            return Result.Error("User cannot sign in without a confirmed account.");
        return Result.Success();
    }

    private async Task<Result<string>> SignInUser(ApplicationUser applicationUser, LoginRequestDTO loginRequest)
    {
        var loginResult =
            await _signInManager.PasswordSignInAsync(applicationUser, loginRequest.Password, loginRequest.RememberMe,
                false);
        if (!loginResult.Succeeded)
            return Result.Error("Unable to login.");
        return Result.Success(applicationUser.UserName);
    }
}