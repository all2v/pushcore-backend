using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.IdentityServer.DomainServices.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;

namespace Astrum.IdentityServer.DomainServices.Features.Commands
{
    /// <summary>
    ///     Token Refresh command
    /// </summary>
    public sealed class TokenRefreshCommand : Command<Result<TokenOperationResult>>
    {
        /// <summary>
        ///     Token Refresh
        /// </summary>
        public TokenRefreshCommand(string refreshToken)
        {
            this.RefreshToken = refreshToken;
        }

        /// <summary>Refresh token</summary>
        public string RefreshToken { get; set; }
    }
}
