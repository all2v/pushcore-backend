using Hangfire.Dashboard;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Astrum.Api.Extensions
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private readonly string[] _roles;

        public HangfireAuthorizationFilter(params string[] roles)
        {
            _roles = roles;
        }

        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var role = httpContext.User.FindFirstValue(ClaimTypes.Role);
            return _roles.Contains(role);
        }
    }
}
