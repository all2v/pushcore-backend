using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.IdentityServer.Domain.ViewModels;
using Astrum.IdentityServer.DomainServices.Features.Commands;
using Astrum.IdentityServer.DomainServices.ViewModels;

namespace Astrum.IdentityServer.Application.Services
{
    public interface IKeycloakExtendedClient
    {
        Task<UserViewModel> CreateUser(UserCreateCommand userCreateRequest);
        Task<ResetUserPasswordResult> ResetUserPassword(string userId, string newPassword);
        Task<TokenOperationResult> RequestOnTokenEndpoint(string grantType, Dictionary<string, string> credentials);
        Task<AuthOperationResult> RequestOnAuthEndpoint();
    }
}
