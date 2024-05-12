using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Astrum.Identity.Contracts;
using Astrum.Identity.Models;
using Astrum.SharedLib.Common.Enumerations;
using Astrum.SharedLib.Common.Extensions;
using Astrum.SharedLib.Domain.Enums;
using Duende.IdentityServer.Extensions;
using Microsoft.AspNetCore.Http;

namespace Astrum.Identity.Services;

// public class KeycloakAuthenticatedUserService : IAuthenticatedUserService
// {
//     public KeycloakAuthenticatedUserService(IHttpContextAccessor httpContextAccessor)
//     {
//         if (httpContextAccessor.HttpContext != null && 
//             httpContextAccessor.HttpContext.User != null &&
//             httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
//         {
//             var userIdExist =
//                 Guid.TryParse(httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value,
//                     out var userId);
//             UserId = userIdExist ? userId : null;
//             Username = httpContextAccessor.HttpContext?.User?.FindFirst("preferred_username")?.Value ?? null;
//         }
//         else
//         {
//             try
//             {
//                 var authHeader = httpContextAccessor.HttpContext.Request.Headers["Authorization"];
//                 var splittedToken = authHeader.FirstOrDefault()?.Split(' ');
//                 if (splittedToken != null && splittedToken.Length == 2 && splittedToken[0] == "Bearer")
//                 {
//                     var token = splittedToken[1];
//                     var handler = new JwtSecurityTokenHandler();
//                     var jsonToken = handler.ReadToken(token);
//                     var tokenS = jsonToken as JwtSecurityToken;
//                     UserId = Guid.Parse(tokenS.Claims.FirstOrDefault(c => c.Type == "sub")?.Value);
//                     Username = tokenS.Claims.FirstOrDefault(c => c.Type == "preferred_username")?.Value;
//                 }
//             }
//             catch (Exception e)
//             {
//                 Console.WriteLine(e);
//             }
//         }
//     }
//
//
//     #region IAuthenticatedUserService Members
//
//     public Guid? UserId { get; }
//     public string? Username { get; }
//     // public IEnumerable<RolesEnum> Roles { get; }
//
//     #endregion
// }