using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.IdentityServer.DomainServices.Features.Commands;
using Astrum.IdentityServer.DomainServices.Services;
using Astrum.IdentityServer.DomainServices.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using IdentityModel.Client;

namespace Astrum.IdentityServer.Application.EventHandlers
{
    public class TokenRefreshCommandHandler : CommandHandler<TokenRefreshCommand, Result<TokenOperationResult>>
    {
        private readonly IUserService userService;

        public TokenRefreshCommandHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public override async Task<Result<TokenOperationResult>> Handle(TokenRefreshCommand command, CancellationToken cancellationToken = default)
        {
            var result = await userService.RefreshBearerToken(command.RefreshToken);
            if (result == null)
                return Result.Invalid(new List<ValidationError> { new ValidationError { ErrorCode = "400", ErrorMessage = "Unknown error", Identifier = "0" } });
            if (result.Successful)
                return Result.Success(result);
            return Result.Invalid(new List<ValidationError> { new ValidationError { ErrorCode = "400", ErrorMessage = result.ErrorMessage, Identifier = "0" } });
        }
    }

}
