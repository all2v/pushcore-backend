using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.IdentityServer.Application.Services;
using Astrum.IdentityServer.DomainServices.Features.Commands;
using Astrum.IdentityServer.DomainServices.Services;
using Astrum.IdentityServer.DomainServices.ViewModels;
using Astrum.SharedLib.Common.CQS.Implementations;
using Astrum.SharedLib.Common.Results;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Sdk.Admin.Models;
using MassTransit.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Astrum.IdentityServer.Application.EventHandlers
{
    public sealed class ResetPasswordCommandHandler : CommandResultHandler<ResetPasswordCommand>
    {
        private readonly IKeycloakExtendedClient _keycloakClient;
        private readonly IUserService _userService;

        public ResetPasswordCommandHandler(IKeycloakExtendedClient keycloakClient, IUserService userService)
        {
            _keycloakClient = keycloakClient;
            _userService = userService;
        }

        public override async Task<Result> Handle(ResetPasswordCommand command, CancellationToken cancellationToken = default)
        {
            var loginWithCurrentPassword = await ValidateCurrentPassword(command.Username, command.CurrentPassword);
            if (!loginWithCurrentPassword.Successful)
                return Result.Error(loginWithCurrentPassword.ErrorMessage);

            var user = await _userService.GetUserByUsername(command.Username);
            if (user == null)
                return Result.NotFound($"User {command.Username} not found");

            var resetPasswordRequest = await _keycloakClient.ResetUserPassword(user.Id.ToString(), command.NewPassword);
            if (!resetPasswordRequest.Successful)
                return Result.Error(resetPasswordRequest.ErrorMessage);

            return Result.Success();
        }

        private async Task<TokenOperationResult> ValidateCurrentPassword(string username, string password)
        {
            var credentials = new Dictionary<string, string>
            {
                { "username", username },
                { "password", password },
            };
            var result = await _keycloakClient.RequestOnTokenEndpoint("password", credentials);
            return result;
        }
    }
}
