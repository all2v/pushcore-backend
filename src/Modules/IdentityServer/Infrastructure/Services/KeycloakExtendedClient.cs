using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.IdentityServer.Application.Services;
using Astrum.IdentityServer.Domain.ViewModels;
using Astrum.IdentityServer.DomainServices.ViewModels;
using Keycloak.AuthServices.Authentication;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin;
using Microsoft.Extensions.Options;
using NetBox.Extensions;
using Newtonsoft.Json;
using Astrum.IdentityServer.DomainServices.Features.Commands;
using static IdentityModel.OidcConstants;
using System.Net;
using Astrum.IdentityServer.DomainServices.Services;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;
using Astrum.SharedLib.Common.Results;

namespace Astrum.IdentityServer.Infrastructure.Services
{
    public class KeycloakExtendedClient : IKeycloakExtendedClient
    {
        private readonly KeycloakAuthenticationOptions _keycloakAuthenticationOptions;
        private readonly IKeycloakUserClient _keycloakUserClient;

        public KeycloakExtendedClient(IOptions<KeycloakAuthenticationOptions> keycloakAuthenticationOptions,
            IKeycloakUserClient keycloakUserClient)
        {
            _keycloakAuthenticationOptions = keycloakAuthenticationOptions.Value;
            _keycloakUserClient = keycloakUserClient;
        }

        public async Task<UserViewModel> CreateUser(UserCreateCommand userCreateRequest)
        {
            var credentials = new Credential[]
            {
                new Credential()
                {
                    Type = "password",
                    Value = userCreateRequest.Password,
                    Temporary = false
                }
            };
            var identityUser = new User()
            {
                FirstName = userCreateRequest.FirstName,
                LastName = userCreateRequest.LastName,
                Username = userCreateRequest.Username,
                Email = userCreateRequest.Email,
                Enabled = true,
                Credentials = credentials
            };
            // TODO create our own keycloak client implementation with necessary methods 
            var resp = await _keycloakUserClient.CreateUser(_keycloakAuthenticationOptions.Realm, identityUser);
            var id = resp.Headers.Location.AbsoluteUri.Split('/').LastOrDefault();
            var succeedIdParse = Guid.TryParse(id, out var guidId);
            if (!succeedIdParse)
                return null;
            var userVM = new UserViewModel
            {
                Username = identityUser.Username,
                Email = identityUser.Email,
                FirstName = identityUser.FirstName,
                LastName = identityUser.LastName,
                Id = guidId
            };
            return userVM;
        }

        public async Task<TokenOperationResult> RequestOnTokenEndpoint(string grantType, Dictionary<string, string> credentials)
        {
            using (var client = new HttpClient())
            {
                var result = new TokenOperationResult();

                var values = new Dictionary<string, string>
                {
                    { "grant_type", grantType },
                    { "client_id", _keycloakAuthenticationOptions.Resource },
                    { "client_secret", _keycloakAuthenticationOptions.Credentials.Secret }
                };
                values.AddRange(credentials);

                var content = new FormUrlEncodedContent(values);

                var url = $"{_keycloakAuthenticationOptions.AuthServerUrl}realms/astrum/protocol/openid-connect/token";
                var response = await client.PostAsync(url, content);

                var responseString = await response.Content.ReadAsStringAsync();
                dynamic? tmp = JsonConvert.DeserializeObject<dynamic>(responseString);
                var error = tmp["error"];
                var unknwnErrMsg = "Unknown error";
                if (error != null)
                { 
                    result.ErrorMessage = tmp["error_description"];
                    if (result.ErrorMessage == null)
                        result.ErrorMessage = unknwnErrMsg;
                }
                else
                {
                    var token = tmp["access_token"];
                    var refreshToken = tmp["refresh_token"];
                    if ((grantType == "client_credentials" && token != null) || (token != null && refreshToken != null))
                    {
                        result.Successful = true;
                        result.AccessToken = token;
                        result.RefreshToken = refreshToken;
                    }
                    else
                        result.ErrorMessage = unknwnErrMsg;
                }
                return result;
            }
        }
        
        public async Task<AuthOperationResult> RequestOnAuthEndpoint()
        {
            using (var client = new HttpClient())
            {
                var result = new AuthOperationResult();

                var requestState = "aj8o3m7bdy1op8";
                
                var values = new Dictionary<string, string>
                {
                    { "client_id", _keycloakAuthenticationOptions.Resource },
                    { "response_type", "code" },
                    { "state", requestState }
                };
                
                var parameters = string.Join("&", values.Select(p => $"{p.Key}={p.Value}"));
                
                var url = $"{_keycloakAuthenticationOptions.AuthServerUrl}realms/astrum/protocol/openid-connect/auth?{parameters}";
                var response = await client.GetAsync(url);

                var responseString = await response.Content.ReadAsStringAsync();
                dynamic? tmp = JsonConvert.DeserializeObject<dynamic>(responseString);
                var error = tmp["error"];
                var unknwnErrMsg = "Unknown error";
                if (error != null)
                {
                    result.ErrorMessage = tmp["error_description"];
                    if (result.ErrorMessage == null)
                        result.ErrorMessage = unknwnErrMsg;
                }
                else
                {
                    var state = tmp["state"];
                    var sessionState = tmp["refresh_token"];
                    var code = tmp["code"];
                    if (state != null && sessionState != null && code != null)
                    {
                        result.Successful = true;
                        result.State = state;
                        result.SessionState = sessionState;
                        result.Code = code;
                    }
                    else
                        result.ErrorMessage = unknwnErrMsg;
                }
                return result;
            }
        }

        public async Task<ResetUserPasswordResult> ResetUserPassword(string userId, string newPassword)
        {
            var credentials = new Dictionary<string, string>
            {
                { "type", "password" },
                { "temporary", "false" },
                { "value", newPassword }
            };

            var adminClientTokenRequest = await RequestOnTokenEndpoint("client_credentials", new());
            if (!adminClientTokenRequest.Successful)
                return new ResetUserPasswordResult { Successful = false, ErrorMessage = adminClientTokenRequest.ErrorMessage };

            var result = new ResetUserPasswordResult();

            using (var client = new HttpClient())
            {
                var content = new StringContent(JsonConvert.SerializeObject(credentials), Encoding.UTF8, "application/json");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", adminClientTokenRequest.AccessToken);

                var url = $"{_keycloakAuthenticationOptions.AuthServerUrl}admin/realms/{_keycloakAuthenticationOptions.Realm}/users/{userId}/reset-password";
                var response = await client.PutAsync(url, content);

                var responseString = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    result.ErrorMessage = responseString;
                    result.Successful = false;
                    return result;
                }

                result.Successful = true;
            }

            return result;
        }
    }
}
