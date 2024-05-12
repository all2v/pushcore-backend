using Astrum.Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Astrum.Identity;

public class ApplicationUserAccessor : IApplicationUserAccessor
{
    private readonly HttpContext _httpContext;
    private readonly UserManager<ApplicationUser> _userManager;

    public ApplicationUserAccessor(UserManager<ApplicationUser> userManager, IHttpContextAccessor contextAccessor)
    {
        _userManager = userManager;
        _httpContext = contextAccessor.HttpContext ?? throw new Exception("HttpContext not found");
    }

    #region IApplicationUserAccessor Members

    public async Task<ApplicationUser?> GetUser()
    {
        var contextUser = _httpContext.User;
        return await _userManager.GetUserAsync(contextUser);
    }

    #endregion
}