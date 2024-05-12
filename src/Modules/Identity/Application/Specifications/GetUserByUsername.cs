using Ardalis.Specification;
using Astrum.Identity.Models;

namespace Astrum.Identity.Specifications;

public class GetUserByUsernameSpec : Specification<ApplicationUser>
{
    public GetUserByUsernameSpec(string username)
    {
        Query
            .Where(x => x.UserName.ToLower() == username.ToLower())
            .Include(x => x.Roles);
    }
}