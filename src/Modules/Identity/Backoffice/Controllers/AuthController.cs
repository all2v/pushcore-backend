using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Security.Claims;
using Astrum.Identity.Application.Contracts;
using Astrum.Identity.Application.ViewModels;
using Astrum.Identity.Application.ViewModels.Requests;
using Astrum.Identity.Features.Commands.ChangePassword;
using Astrum.Identity.Features.Commands.Login;
using Astrum.Identity.Features.Commands.Logout;
using Astrum.Identity.Features.Commands.Register;
using Astrum.Identity.Managers;
using Astrum.Infrastructure.Shared;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.SharedLib.Common.Results;
using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NetBox.Extensions;
using Newtonsoft.Json;
using static IdentityModel.OidcConstants;
using Astrum.Identity.Models;
using Astrum.Identity.Domain.Entities;
using Astrum.Identity.Contracts;
using static MassTransit.ValidationResultExtensions;
using Result = Astrum.SharedLib.Common.Results.Result;
using Microsoft.AspNetCore.Authentication.Cookies;
using Astrum.SharedLib.Application.Helpers;
using Astrum.Logging.Services;
using AutoMapper;
using Astrum.Logging.Entities.LogEntities;

namespace Astrum.Identity.Controllers;

/// <summary>
/// </summary>
[Route("[controller]")]
public class AuthController : ApiBaseController
{
    private readonly IDataProtector _dataProtector;
    private readonly ApplicationUserManager _userManager;
    private readonly ApplicationSignInManager _signInManager;
    private readonly IIdentityJwtGenerator _jwtGenerator;
    private readonly IGitlabMappingService _gitlabMappingService;
    private readonly IUserAuthenticationService _userAuthenticationService;
    private readonly string DefaultAdminRedirectPath = "/admin/banner";
    private readonly ILogHttpService _logger;
    private readonly IMapper _mapper;

    public AuthController(IDataProtectionProvider dataProtectionProvider, ApplicationUserManager userManager,
        ApplicationSignInManager signInManager, IIdentityJwtGenerator jwtGenerator,
        IGitlabMappingService gitlabMappingService, IUserAuthenticationService userAuthenticationService, ILogHttpService logger, IMapper mapper)
    {
        _dataProtector = dataProtectionProvider.CreateProtector("SignIn");
        _userManager = userManager;
        _signInManager = signInManager;
        _jwtGenerator = jwtGenerator;
        _gitlabMappingService = gitlabMappingService;
        _userAuthenticationService = userAuthenticationService;
        _logger = logger;
        _mapper = mapper;
    }

    /// <summary>
    ///     Authentication via username or password
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    [AllowAnonymous]
    [HttpPost("[action]")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<UserTokenResult>))]
    [ProducesResponseType(typeof(Result<UserTokenResult>), StatusCodes.Status200OK)]
    public async Task<Result<UserTokenResult>> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(command, cancellationToken);
        if (response.IsSuccess)
        {
            _logger.Log(command.Login, Result.Success("Вход успешен."), HttpContext, "Пользователь авторизован.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Identity);
        }
        else
        {
            _logger.Log(command.Login, response, HttpContext, "Пользователь авторизован.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Identity);
        }
        return response;
    }

    //[AllowAnonymous]
    //[HttpGet("admin-basic-login")]
    //public async Task<IActionResult> AdminBasicLogin(string protectedData)
    //{
    //    var data = _dataProtector.Unprotect(protectedData);

    //    var parts = data.Split('|');
    //    var identityUser = await _userManager.FindByIdAsync(parts[0]);

    //    if (identityUser == null)
    //    {
    //        return Unauthorized();
    //    }

    //    //if (!await _userManager.IsInRoleAsync(identityUser, "ADMIN") || !await _userManager.IsInRoleAsync(identityUser, "SUPERADMIN"))
    //    //    return Forbid();

    //    var isTokenValid = await _userManager.VerifyUserTokenAsync(identityUser, TokenOptions.DefaultProvider, "SignIn", parts[1]);

    //    if (isTokenValid)
    //    {
    //        await _signInManager.SignInAsync(identityUser, true, CookieAuthenticationDefaults.AuthenticationScheme);
    //        if (parts.Length == 3 && Url.IsLocalUrl(parts[2]))
    //        {
    //            return Redirect(parts[2]);
    //        }
    //        return Redirect(DefaultAdminRedirectPath);
    //    }
    //    else
    //    {
    //        return Unauthorized();
    //    }
    //}

    [AllowAnonymous]
    [HttpGet("admin-login")]
    public async Task<IActionResult> AdminLogin(string data)
    {
        var base64EncodedBytes = System.Convert.FromBase64String(data);
        var token = System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        //var authHeader = HttpContext.Request.Headers["Authorization"];
        //var splittedToken = authHeader.FirstOrDefault()?.Split(' ');
        //if (!(splittedToken != null && splittedToken.Length == 2 && splittedToken[0] == "Bearer"))
        //{
        //    return Unauthorized();
        //}
        //var token = splittedToken[1];
        if (!_jwtGenerator.ValidateToken(token))
        {
            _logger.Log("[Секретные данные]", Result.Invalid(new List<ValidationError>() { new ValidationError { ErrorMessage = "Невалидный токен для входа в админку.", ErrorCode = "401" } }),
                HttpContext, "Пользователь вошёл в админку.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Identity);
            return Unauthorized();
        }
        var claims = JwtManager.GetJwtTokenClaims(token);
        if (claims == null) {
             _logger.Log("[Секретные данные]", Result.NotFound("Ошибка, клеймов для входа нет."), HttpContext,
                "Пользователь вошёл в админку.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Identity);
             return Unauthorized();
        }
        if (!Guid.TryParse(claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value, out var userId)) {
            _logger.Log("[Секретные данные]", Result.NotFound("Ошибка, нужных клеймов для входа нет."), HttpContext,
                "Пользователь вошёл в админку.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Identity);
            return Unauthorized();
        }
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null) 
        {
            _logger.Log("[Секретные данные]", Result.NotFound("Пользователь не найден."), HttpContext, "Пользователь вошёл в админку.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Identity);
            return Unauthorized(); 
        }
        await _signInManager.SignInAsync(user, true, CookieAuthenticationDefaults.AuthenticationScheme);
        _logger.Log("[Секретные данные]", Result.Success(_mapper.Map<UserLogs>(user)), HttpContext, "Пользователь вошёл в админку.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Identity);
        return Redirect(DefaultAdminRedirectPath);
    }

    /// <summary>
    ///     Logout user
    /// </summary>
    /// <returns></returns>
    [HttpPost("[action]")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<Result> Logout([FromBody] LogoutCommand command, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(command, cancellationToken);
        return response;
    }

    /// <summary>
    ///     Change password
    /// </summary>
    /// <param name="command">Request body</param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [HttpPost("change-password")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result))]
    [ProducesResponseType(typeof(Result), StatusCodes.Status200OK)]
    public async Task<Result> ChangePassword([FromBody] ChangePasswordCommand command,
        CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(command, cancellationToken);
        _logger.Log("[Секретные данные]", response, HttpContext, "Изменён пароль пользователя.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Identity);
        return response;
    }

    /// <summary>
    ///     Registration
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    [AllowAnonymous]
    [HttpPost("[action]")]
    [TranslateResultToActionResult]
    [ProducesDefaultResponseType(typeof(Result<ApplicationUser>))]
    [ProducesResponseType(typeof(Result<ApplicationUser>), StatusCodes.Status200OK)]
    public async Task<Result<ApplicationUser>> Register([FromBody] RegisterCommand command, CancellationToken cancellationToken)
    {
        var response = await Mediator.Send(command, cancellationToken);
        var log = response.Map(_mapper.Map<ApplicationUser, UserLogs>);
        _logger.Log(command.Username, log, HttpContext, "Пользователь зарегистрирован.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Identity);
        return response;
    }

    //[AllowAnonymous]
    //[HttpGet("gitlabauthenticate")]
    //public IActionResult LoginWithGitlab(string? returnUrl)
    //{
    //    //return new ChallengeResult(
    //    //    "GitLab",
    //    //    new AuthenticationProperties
    //    //    {
    //    //        RedirectUri = string.IsNullOrWhiteSpace(returnUrl) ? Url.Action(nameof(LoginCallback)) :
    //    //        Url.Action(nameof(LoginCallback), new { returnUrl })
    //    //    });
    //    var provider = "GitLab";
    //    var root = $"/api/auth/gitlabcallback?returnUrl={returnUrl ?? string.Empty}";
    //    var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, root);
    //    return Challenge(properties, provider);
    //}

    //[AllowAnonymous]
    //[HttpGet("gitlabcallback")]
    //public async Task<IActionResult> LoginCallback(string? returnUrl)
    //{
    //    //var authenticateResult = await HttpContext.AuthenticateAsync("External");
    //    var info = await _signInManager.GetExternalLoginInfoAsync();
    //    if (info == null)
    //    {
    //        throw new InvalidOperationException("Unable to get external login!");
    //        //return Unauthorized();
    //    }
    //    //var token = await HttpContext.GetClientAccessTokenAsync();
    //    //if (!authenticateResult.Succeeded)
    //    //    //return Result.Invalid(new List<ValidationError> { new ValidationError { ErrorCode = "400", ErrorMessage = "Unknown error", Identifier = "0" } });
    //    //    return Unauthorized();

    //    var loginProvider = info.LoginProvider;
    //    var providerKey = info.ProviderKey;
    //    var claims = info.Principal.Claims;
    //    //var claims = authenticateResult.Principal.Claims;
    //    var gitlabUser = new GitLabUserForm
    //    {
    //        Id = claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value,
    //        Email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value,
    //        Name = claims.FirstOrDefault(c => c.Type == "urn:gitlab:name")?.Value,
    //        Username = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value,
    //        IsBot = false,
    //        IsExternal = false
    //    };
    //    var createUserResult = await _userAuthenticationService.GetOrCreateUserByGitlabId(gitlabUser);
    //    if (!createUserResult.Successful || createUserResult.User == null)
    //        throw new InvalidOperationException("User creation unsuccessfull!");
    //    //return Unauthorized();

    //    await _signInManager.SignInAsync(createUserResult.User, true, CookieAuthenticationDefaults.AuthenticationScheme);
    //    //Logging externally doesn't work, I don't know why(
    //    //var identityResult = await _userManager.AddLoginAsync(createUserResult.User, info);
    //    //var signInResult = await _signInManager.ExternalLoginSignInAsync(loginProvider, providerKey, true);
    //    //if (!signInResult.Succeeded || identityResult.Errors.Any(x => x.Code != "LoginAlreadyAssociated"))
    //    //    return Unauthorized();
    //    //await _signInManager.SignInAsync(createUserResult.User, true);
    //    //await HttpContext.SignInAsync(
    //    //    CookieAuthenticationDefaults.AuthenticationScheme,
    //    //    new ClaimsPrincipal(createUserResult.User.));
    //    return Redirect(DefaultAdminRedirectPath);
    //}

    [AllowAnonymous]
    [HttpPost("gitlab-code-auth")]
    public async Task<Result<TokenOperationResult>> LoginWithGitlabCode([FromBody] GitlabAuthRequest request)
    {
        //TODO: refactor
        using (var client = new HttpClient())
        {
            var values = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                //TODO: hide in appsettings
                { "client_id", "d56910f55789568362d2a44dbf22b43bb8a42e927a87a1ade9c5fe4bc4400148" },
                { "client_secret", "b4a29df890d1df8ac2c5a1c09af8b0293e1a3fb29d99cf325d36fd1c50ccd908" },
                { "redirect_uri", request.RedirectUri },
                { "code", request.Code }
            };

            var content = new FormUrlEncodedContent(values);

            var url = $"https://git.66bit.ru/oauth/token";
            var response = await client.PostAsync(url, content);
            var responseString = await response.Content.ReadAsStringAsync();
            dynamic? tmp = JsonConvert.DeserializeObject<dynamic>(responseString);
            var error = tmp["error"];
            var unknwnErrMsg = "Unknown error";
            if (error != null)
            {
                var errorMsg = (string)tmp["error_description"];
                if (errorMsg == null)
                    errorMsg = unknwnErrMsg;
                _logger.Log("[Секретные данные]", Result.Error(errorMsg, "Ошибка во-время входа через GitLab."), HttpContext, "Вошёл пользователь через Gitlub.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Identity);
                return Result.Error(errorMsg);
            }
            else
            {
                var gitlabToken = (string)tmp["access_token"];
                var gitlabRefreshToken = (string)tmp["refresh_token"];
                if (gitlabToken == null || gitlabRefreshToken == null)
                {
                    _logger.Log("[Секретные данные]", Result.Error(unknwnErrMsg, "Ошибка во-время входа через GitLab."), HttpContext, "Вошёл пользователь через Gitlub.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Identity);
                    return Result.Error(unknwnErrMsg);

                }
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", gitlabToken);
                var userResponse = await client.GetAsync("https://git.66bit.ru/api/v4/user");
                var userResponseString = await userResponse.Content.ReadAsStringAsync();
                //dynamic? userTmp = JsonConvert.DeserializeObject<dynamic>(userResponseString);
                //bool isBot = userTmp["bot"];
                //bool isExternal = userTmp["external"];

                //string gitlabId = userTmp["id"];
                //string gitlabUserName = userTmp["username"];
                //string gitlabName = userTmp["name"];
                //string gitlabEmail = userTmp["email"];
                var userTmp = JsonConvert.DeserializeObject<GitLabUserForm>(userResponseString);
                //TODO: rework on external logins
                var result = await _userAuthenticationService.LoginGitlabUser(userTmp);
                if (result.Successful)
                {
                    _logger.Log("[Секретные данные]", Result.Success("[Секретные данные]"), HttpContext, "Вошёл пользователь через GitLab.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Identity);
                    return Result.Success(result);
                }
                _logger.Log("[Секретные данные]", Result.Error(result.ErrorMessage, "Ошибка во-время входа через GitLab."), HttpContext, "Вошёл пользователь через GitLab.", Logging.Entities.TypeRequest.POST, Logging.Entities.ModuleAstrum.Identity);
                return Result.Error(result.ErrorMessage);
            }
        }
    }

    [AllowAnonymous]
    [HttpGet("logout")]
    public async Task<IActionResult> BlazorLogout()
    {
        await _signInManager.SignOutAsync();
        return Redirect("/");
    }
}