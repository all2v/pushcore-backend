using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace Api.Application.Authorization.Abstractions.Impl;

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    #region ICurrentUserService Members

    public string? UserId => _httpContextAccessor
        .HttpContext
        ?.User
        ?.FindFirstValue(ClaimTypes.NameIdentifier);

    public string? UserName => _httpContextAccessor
        .HttpContext
        ?.User
        ?.FindFirstValue("preferred_username"); // TODO to keycloak constant

    public ClaimsPrincipal? Principal => _httpContextAccessor
        .HttpContext
        ?.User;

    #endregion
}