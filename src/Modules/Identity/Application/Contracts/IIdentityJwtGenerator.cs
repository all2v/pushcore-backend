using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Astrum.Identity.Application.ViewModels;
using Astrum.Identity.Models;

namespace Astrum.Identity.Application.Contracts
{
    public interface IIdentityJwtGenerator
    {
        string CreateToken(ApplicationUser user, IEnumerable<string> roles = null);
        string CreateToken(TokenGenerationForm user);
        bool ValidateToken(string authToken);
    }
}
