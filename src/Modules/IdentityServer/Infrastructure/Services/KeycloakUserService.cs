using Astrum.IdentityServer.DomainServices.Services;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Authentication.Configuration;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Microsoft.Extensions.Options;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;
using System.Net;
using Astrum.IdentityServer.DomainServices.ViewModels;
using Astrum.IdentityServer.Domain.Events;
using System.Threading;
using MediatR;
using IdentityModel.Client;
using Keycloak.AuthServices.Sdk.AuthZ;
using Newtonsoft.Json;
using System.Xml.Linq;
using Astrum.Account.Aggregates;
using Astrum.Account.Enums;
using Astrum.SharedLib.Application.Contracts.Infrastructure.Shared;
using Astrum.SharedLib.Application.Contracts.Infrastructure;
using Astrum.SharedLib.Application.Models;
using Astrum.Account.Specifications.UserProfile;
using System;
using MassTransit.Initializers;
using Astrum.IdentityServer.Domain.ViewModels;
using AutoMapper;
using Astrum.IdentityServer.Application.Services;
using Astrum.IdentityServer.DomainServices.Features.Commands;

namespace Api.Application.Authorization.Abstractions.Impl;

public class KeycloakUserService : IUserService
{
    private readonly IKeycloakUserClient _keycloakUserClient;
    private readonly IKeycloakExtendedClient _keycloakExtendedClient;
    private readonly IMapper _mapper;
    private readonly KeycloakAuthenticationOptions _keycloakAuthenticationOptions;
    private readonly IMediator _mediator;

    public KeycloakUserService(IKeycloakUserClient keycloakUserClient, IMediator mediator,
        IOptions<KeycloakAuthenticationOptions> keycloakAuthenticationOptions,
        IMapper mapper, IKeycloakExtendedClient keycloakExtendedClient)
    {
        _keycloakUserClient = keycloakUserClient;
        _keycloakAuthenticationOptions = keycloakAuthenticationOptions.Value;
        _mediator = mediator;
        _mapper = mapper;
        _keycloakExtendedClient= keycloakExtendedClient;
    }

    public async Task<UserViewModel> CreateUser(UserCreateCommand user)
    {
        var createdUser = await _keycloakExtendedClient.CreateUser(user);
        await _mediator.Publish(new ApplicationUserCreatedEvent(createdUser.Id.ToString(),
            1, createdUser, null, null, user.Password));
        return createdUser;
    }

    public async Task<UserViewModel> GetUser(string userId)
    {
        User? user = null;
        try
        {
            user = await _keycloakUserClient.GetUser(_keycloakAuthenticationOptions.Realm, userId);
        }
        catch
        {
            return null;
        }
        if (user == null)
            return null;
        return new UserViewModel
        {
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Id = Guid.Parse(user.Id)
        };
    }

    public async Task<UserViewModel> GetUserByUsername(string username)
    {
        var parameters = new GetUsersRequestParameters()
        {
            Username = username,
        };
        var user = (await _keycloakUserClient.GetUsers(_keycloakAuthenticationOptions.Realm, parameters)).FirstOrDefault();
        if (user == null)
            return null;
        return new UserViewModel
        {
            Username = user.Username,
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Id = Guid.Parse(user.Id)
        };
    }
    
    public async Task<UserTokenResult> GetBearerTokenByPassword(LoginByPasswordCommand loginRequest)
    {
        // var code = await _keycloakExtendedClient.RequestOnAuthEndpoint();
        // if (!code.Successful)
        //     return new UserTokenResult() { Successful = false, ErrorMessage = code.ErrorMessage };
        
        var credentials = new Dictionary<string, string>
        {
            { "password", loginRequest.Password },
            { "username", loginRequest.Username }
        };
        var token = await _keycloakExtendedClient.RequestOnTokenEndpoint("password", credentials);
        var result = _mapper.Map<UserTokenResult>(token);
        var user = await GetUserByUsername(loginRequest.Username);
        // result.Id = user.Id;
        // result.Username = user.Username;
        return result;
    }
    
    public async Task<UserTokenResult> GetBearerTokenFromAuthCode(LoginCommand loginRequest)
    {
        // var code = await _keycloakExtendedClient.RequestOnAuthEndpoint();
        // if (!code.Successful)
        //     return new UserTokenResult() { Successful = false, ErrorMessage = code.ErrorMessage };
        
        var credentials = new Dictionary<string, string>
        {
            { "redirect_uri", loginRequest.ReturnUrl },
            { "code", loginRequest.Code }
        };
        var token = await _keycloakExtendedClient.RequestOnTokenEndpoint("authorization_code", credentials);
        var result = _mapper.Map<UserTokenResult>(token);
        return result;
    }

    public async Task<TokenOperationResult> RefreshBearerToken(string inputRefreshToken)
    {
        var credentials = new Dictionary<string, string>
        {
            { "refresh_token", inputRefreshToken }
        };
        return await _keycloakExtendedClient.RequestOnTokenEndpoint("refresh_token", credentials);
    }

    public async Task<IEnumerable<UserViewModel>> GetUsers(GetUsersRequestParameters parameters = null)
    {
        return (await _keycloakUserClient.GetUsers(_keycloakAuthenticationOptions.Realm, parameters)).Select(u =>
        {
            var user = new UserViewModel
            {
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Id = Guid.Parse(u.Id)
            };
            return user;
        });
    }

    //public async Task<UserViewModel> EditUser(UserEditCommand user)
    //{
    //    if (user == null)
    //        return null;
    //    var keycloakUser = await _keycloakUserClient.GetUser(_keycloakAuthenticationOptions.Realm, user.Id.ToString());
    //    if (keycloakUser == null)
    //        return null;
    //    var keycloakUserToUpdate = new User
    //    {
    //        Id = keycloakUser.Id,
    //        Email = user.Email,
    //        FirstName = user.FirstName,
    //        LastName = user.LastName,
    //        Username = user.Username,
    //        Access = keycloakUser.Access,
    //        FederatedIdentities = keycloakUser.FederatedIdentities,
    //        FederationLink = keycloakUser.FederationLink,
    //        Attributes = keycloakUser.Attributes,
    //        ClientConsents = keycloakUser.ClientConsents,
    //        ClientRoles = keycloakUser.ClientRoles,
    //        CreatedTimestamp = keycloakUser.CreatedTimestamp,
    //        Credentials = keycloakUser.Credentials,
    //        DisableableCredentialTypes = keycloakUser.DisableableCredentialTypes,
    //        EmailVerified = keycloakUser.EmailVerified,
    //        Enabled = keycloakUser.Enabled,
    //        Groups = keycloakUser.Groups,
    //        Origin = keycloakUser.Origin,
    //        NotBefore = keycloakUser.NotBefore,
    //        RealmRoles = keycloakUser.RealmRoles,
    //        RequiredActions = keycloakUser.RequiredActions,
    //        Self = keycloakUser.Self,
    //        ServiceAccountClientId = keycloakUser.ServiceAccountClientId,
    //        Totp = keycloakUser.Totp
    //    };
    //    await _keycloakUserClient.UpdateUser(_keycloakAuthenticationOptions.Realm, user.Id.ToString(), keycloakUserToUpdate);
    //    return _mapper.Map<UserViewModel>(user);
    //}

    public Task SendVerifyEmail()
    {
        throw new NotImplementedException();
    }
}