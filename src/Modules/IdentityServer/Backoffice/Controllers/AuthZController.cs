//using Astrum.Infrastructure.Shared;
//using System.Security.Claims;
//using Astrum.IdentityServer.Domain.ViewModels;
//using Astrum.IdentityServer.DomainServices.Features.Commands;
//using Astrum.IdentityServer.DomainServices.Services;
//using Astrum.IdentityServer.DomainServices.ViewModels;
//using Astrum.Infrastructure.Shared.Result.AspNetCore;
//using Astrum.SharedLib.Application.Contracts.Infrastructure;
//using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
//using Astrum.SharedLib.Common.Results;
//using Keycloak.AuthServices.Sdk.Admin.Models;
//using Keycloak.AuthServices.Sdk.AuthZ;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;
//using Astrum.SharedLib.Application.Models;
//using Keycloak.AuthServices.Authentication;
//using Microsoft.AspNetCore.Authentication.Cookies;
//using Microsoft.AspNetCore.Authentication.OpenIdConnect;
//using Microsoft.Extensions.Options;

//namespace Api.Controllers;

//[Route("authz")]
//public class KeycloakAuthZController : ApiBaseController
//{
//    private readonly IKeycloakProtectionClient protectionClient;
//    private readonly IUserService userService;
//    private readonly IPasswordGeneratorService passwordGeneratorService;
//    private readonly IEmailService emailService;
//    private readonly OpenIdConnectOptions openIdConnectOptions;

//    public KeycloakAuthZController(IKeycloakProtectionClient protectionClient, IUserService userService,
//        IOptions<OpenIdConnectOptions> openIdConnectOptions,
//        IPasswordGeneratorService passwordGeneratorService, IEmailService emailService)
//    {
//        this.protectionClient = protectionClient;
//        this.userService = userService;
//        this.passwordGeneratorService = passwordGeneratorService;
//        this.emailService = emailService;
//        this.openIdConnectOptions = openIdConnectOptions.Value;
//    }

//    [HttpGet("try-resource")]
//    public async Task<IActionResult> VerifyAccess(
//        [FromQuery] string? resource,
//        [FromQuery] string? scope,
//        CancellationToken cancellationToken)
//    {
//        var verified = await this.protectionClient
//            .VerifyAccessToResource(resource ?? "workspaces", scope ?? "workspaces:read", cancellationToken);

//        return this.Ok(verified);
//    }

//    /// <summary>
//    ///     Get user Bearer by username and password
//    /// </summary>
//    [HttpPost("loginbypassword")]
//    [AllowAnonymous]
//    [TranslateResultToActionResult]
//    [ProducesDefaultResponseType(typeof(Result))]
//    [ProducesResponseType(typeof(List<UserTokenResult>), StatusCodes.Status200OK)]
//    public async Task<Result<UserTokenResult>> Login([FromBody] LoginByPasswordCommand loginRequest, CancellationToken cancellationToken)
//    {
//        return await Mediator.Send(loginRequest);
//    }

    
//    /// <summary>
//    ///     Get user Bearer from keycloak
//    /// </summary>
//    [HttpGet("login")]
//    [AllowAnonymous]
//    // [TranslateResultToActionResult]
//    // [ProducesDefaultResponseType(typeof(Result))]
//    // [ProducesResponseType(typeof(List<UserTokenResult>), StatusCodes.Status200OK)]
//    public IActionResult/*<Result<UserTokenResult>>*/ Login(CancellationToken cancellationToken)
//    {
//        var requestState = "aj8o3m7bdy1op8";
//        var redirectUrl = Uri.EscapeDataString(Url.ActionLink(nameof(LoginCallback)));
//        var values = new Dictionary<string, string>
//        {
//            { "client_id", openIdConnectOptions.Resource },
//            { "response_type", "code" },
//            { "redirect_uri", redirectUrl },
//            { "state", requestState }
//        };
                
//         var parameters = string.Join("&", values.Select(p => $"{p.Key}={p.Value}"));
                
//        var url = $"{openIdConnectOptions.Authority}/protocol/openid-connect/auth?{parameters}";
//        return Redirect(url);
//        // return await Mediator.Send(new LoginCommand());
//    }
    
//    /// <summary>
//    ///     Keycloak authflow Callback
//    /// </summary>
//    [HttpGet("logincallback")]
//    [AllowAnonymous]
//    [TranslateResultToActionResult]
//    [ProducesDefaultResponseType(typeof(Result))]
//    [ProducesResponseType(typeof(List<UserTokenResult>), StatusCodes.Status200OK)]
//    public async Task<Result<UserTokenResult>> LoginCallback([FromQuery] string state, [FromQuery] string session_state,
//        [FromQuery] string code, CancellationToken cancellationToken)
//    {
//        var redirectUrl = Url.ActionLink(nameof(LoginCallback));
//        return await Mediator.Send(new LoginCommand(code, redirectUrl));
//    }
    
//    /// <summary>
//    ///     Refresh user Bearer token by refresh token
//    /// </summary>
//    [HttpPost("refreshToken")]
//    [AllowAnonymous]
//    [TranslateResultToActionResult]
//    [ProducesDefaultResponseType(typeof(Result))]
//    [ProducesResponseType(typeof(List<TokenOperationResult>), StatusCodes.Status200OK)]
//    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
//    public async Task<Result<TokenOperationResult>> RefreshToken([FromBody] TokenRefreshCommand refreshTokenRequest, CancellationToken cancellationToken)
//    {
//        return await Mediator.Send(refreshTokenRequest);
//    }

//    //[HttpGet("account/gitlabauthenticate")]
//    //public IActionResult GitlabAuthenticate(string returnUrl)
//    //{
//    //    var provider = "GitLab";
//    //    var root = $"/account/{nameof(GitlabAuthorization)}?returnUrl={returnUrl ?? string.Empty}";
//    //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, root);
//    //    return Challenge(properties, provider);
//    //}

//    //[HttpGet]
//    //public async Task<IActionResult> GitlabAuthorization(string remoteError, string returnUrl)
//    //{
//    //    var info = await _signInManager.GetExternalLoginInfoAsync();
//    //    if (info == null || remoteError != null)
//    //    {
//    //        return Redirect("/account/signin");
//    //    }
//    //    var loginProvider = info.LoginProvider;
//    //    var providerKey = info.ProviderKey;
//    //    var claims = info.Principal.Claims;
//    //    var user = await _userManager.FindByLoginAsync(loginProvider, providerKey);
//    //    if (user == null)
//    //    {
//    //        var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
//    //        user = await _userManager.FindByEmailAsync(email);
//    //        if (user == null)
//    //        {
//    //            var name = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
//    //            user = new User
//    //            {
//    //                Email = email,
//    //                UserName = name,
//    //                EmailConfirmed = true,
//    //                CompanyProfileId = 1 //TODO: �������� ����������
//    //            };
//    //            if (await _userManager.FindByNameAsync(name) != null)
//    //                user.UserName = email;
//    //            if (await _userManager.FindByNameAsync(email) != null)
//    //                return Content("��� ���������� ������������ � ����� UserName � � UserName ��� ��� Email!");
//    //            var creationResult = await _userManager.CreateAsync(user);
//    //        }
//    //    }
//    //    var identityResult = await _userManager.AddLoginAsync(user, info);
//    //    var signInResult = await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, false);
//    //    if (!signInResult.Succeeded || identityResult.Errors.Any(x => x.Code != "LoginAlreadyAssociated"))
//    //        return Content("�������� ������ �� ������� GitLab!");
//    //    return Redirect($"~{returnUrl ?? "/"}");
//    //}

//    [AllowAnonymous]
//    [HttpGet("signin")]
//    public async Task BlazorLogin(string returnUrl)
//    {
//        await HttpContext.ChallengeAsync(
//            OpenIdConnectDefaults.AuthenticationScheme,
//            new AuthenticationProperties
//            {
//                // RedirectUri = Url.Action(nameof(BlazorLoginCallback), new { returnUrl })
//                RedirectUri = returnUrl
//            });
//    }

//    [AllowAnonymous]
//    [HttpGet("keycloakcallback")]
//    public async Task<IActionResult> BlazorLoginCallback(string returnUrl)
//    {
//        var authenticateResult = await HttpContext.AuthenticateAsync(OpenIdConnectDefaults.AuthenticationScheme);
//        var username = authenticateResult.Principal.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
//        var email = authenticateResult.Principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
        
//        var claims = new List<Claim>
//        {
//            new Claim(ClaimTypes.Name, username),
//            new Claim(ClaimTypes.Email, email),
//        };
//        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
//        await HttpContext.SignInAsync(
//            CookieAuthenticationDefaults.AuthenticationScheme,
//            new ClaimsPrincipal(claimsIdentity));
        
//        if (returnUrl == null)
//            return Redirect("/admin/banner");
//        return Redirect(returnUrl);
//    }

////     [AllowAnonymous]
////     [HttpGet("gitlabauthenticate")]
////     public IActionResult LoginWithGitlab(string returnUrl)
////     {
////         return new ChallengeResult(
////             "GitLab",
////             new AuthenticationProperties
////             {
////                 RedirectUri = Url.Action(nameof(LoginCallback), new { returnUrl })
////             });
////     }
////
////     [AllowAnonymous]
////     [HttpGet("gitlabcallback")]
////     public async Task<Result<TokenOperationResult>> LoginCallback(string returnUrl)
////     {
////         var authenticateResult = await HttpContext.AuthenticateAsync("External");
////         //var token = await HttpContext.GetClientAccessTokenAsync();
////         if (!authenticateResult.Succeeded)
////             return Result.Invalid(new List<ValidationError> { new ValidationError { ErrorCode = "400", ErrorMessage = "Unknown error", Identifier = "0" } });
////
////         var claims = authenticateResult.Principal.Claims;
////         var username = authenticateResult.Principal.Identity.Name;
////         var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
////         var fullName = claims.FirstOrDefault(c => c.Type == "urn:gitlab:name")?.Value;    
////         var keycloakUserByUsername = await userService.GetUserByUsername(username);
////         /*if (keycloakUserByUsername == null)
////         {
////             var passwd = passwordGeneratorService.GenerateRandomPassword();
////             var newUser = new UserCreateRequest
////             {
////                 Username = username,
////                 Email = email,
////                 FirstName = fullName.Split(" ")?.FirstOrDefault() ?? string.Empty,
////                 LastName = fullName.Split(" ")?.Skip(1)?.FirstOrDefault() ?? string.Empty,
////                 Password = passwd
////             };
////             var usr = await userService.CreateUser(newUser);
////             //var keycloakUserByEmail = await userService.GetUserByUsername(email);
////             //if (keycloakUserByEmail == null)
////             //{
////             //    
////             //}
////             var mail = new Email()
////             {
////                 To = email,
////                 Subject = "����������� �� ������� ����� GitLab",
////                 Body = passwd
////             };
////
////             await emailService.SendEmail(mail);
////             var loginRequest = new UserLoginRequest
////             {
////                 Username = username,
////                 Password = passwd
////             };
////             var result = await userService.GetBearerToken(loginRequest);
////             if (result == null)
////                 return Result.Invalid(new List<ValidationError> { new ValidationError { ErrorCode = "400", ErrorMessage = "Unknown error", Identifier = "0" } });
////             if (result.Successful)
////                 return Result.Success(result);
////         }
////
////         return Result.Invalid(new List<ValidationError> { new ValidationError { ErrorCode = "400", ErrorMessage = "Unknown error", Identifier = "0" } });*/
////         return null;
////     }
    
//    [AllowAnonymous]
//    [HttpGet("logout")]
//    public async Task<IActionResult> BlazorLogout()
//    {
//        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
//        return Redirect("/login");
//    }

//    /// <summary>
//    ///     Reset user password
//    /// </summary>
//    [HttpPost("reset-password")]
//    [TranslateResultToActionResult]
//    [ProducesDefaultResponseType(typeof(Result))]
//    public async Task<Result> ResetPassword([FromBody] ResetPasswordCommand command, CancellationToken cancellationToken)
//    {
//        return await Mediator.Send(command, cancellationToken);
//    }
//}
