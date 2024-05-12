using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Astrum.Infrastructure.Shared;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin;
using Microsoft.AspNetCore.Mvc;
using Keycloak.AuthServices.Sdk.Admin;
using Keycloak.AuthServices.Sdk.Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Astrum.IdentityServer.DomainServices.Services;
using Astrum.Infrastructure.Shared.Result.AspNetCore;
using Astrum.SharedLib.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Http;
using Astrum.IdentityServer.DomainServices.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Astrum.IdentityServer.Domain.ViewModels;
using Astrum.IdentityServer.DomainServices.Features.Commands;

// namespace Astrum.IdentityServer.Backoffice.Controllers
// {
//     [Route("api/users")]
//     public class UserController : ApiBaseController
//     {
//         /// <summary>
//         ///     Create user
//         /// </summary>
//         /// <param name="user"></param>
//         /// <returns></returns>
//         [HttpPost]
//         [AllowAnonymous]
//         [TranslateResultToActionResult]
//         [ProducesDefaultResponseType(typeof(Result))]
//         [ProducesResponseType(typeof(UserCreateCommand), StatusCodes.Status200OK)]
//         public async Task<Result<UserViewModel>> Create([FromForm] UserCreateCommand user)
//         {
//             return await Mediator.Send(user);
//         }
//
//         /// <summary>
//         ///     Update user
//         /// </summary>
//         /// <param name="user"></param>
//         /// <returns></returns>
//         [HttpPut]
//         [AllowAnonymous]
//         [TranslateResultToActionResult]
//         [ProducesDefaultResponseType(typeof(Result))]
//         [ProducesResponseType(typeof(UserEditCommand), StatusCodes.Status200OK)]
//         public async Task<Result<UserViewModel>> Update([FromForm] UserEditCommand user)
//         {
//             return await Mediator.Send(user);
//         }
//     }
// }