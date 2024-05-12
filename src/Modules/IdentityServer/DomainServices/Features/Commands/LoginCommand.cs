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
    ///     Login command
    /// </summary>
    public sealed class LoginCommand : Command<Result<UserTokenResult>>
    {
        /// <summary>
        ///     Login command
        /// </summary>
        public LoginCommand(string code, string returnUrl)
        {
            Code = code;
            ReturnUrl = returnUrl;
        }
        
        public string Code { get; set; }
        public string ReturnUrl { get; set; }
    }
}
