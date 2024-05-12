using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.IdentityServer.Domain.ViewModels;
using Astrum.IdentityServer.DomainServices.Features.Commands;
using Astrum.IdentityServer.DomainServices.Services;
using Astrum.IdentityServer.DomainServices.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Astrum.IdentityServer.Application.EventHandlers
{
    public class UserCreateCommandHandler : CommandHandler<UserCreateCommand, Result<UserViewModel>>
    {
        private readonly IUserService userService;

        public UserCreateCommandHandler(IUserService userService)
        {
            this.userService = userService;
        }

        public override async Task<Result<UserViewModel>> Handle(UserCreateCommand command, CancellationToken cancellationToken = default)
        {
            var newUser = await userService.CreateUser(command);
            if (newUser == null)
                return Result.Invalid(new List<ValidationError> { new ValidationError { ErrorCode = "400", ErrorMessage = "Unknown error", Identifier = "0" } });
            return Result.Success(newUser);
        }
    }
}
