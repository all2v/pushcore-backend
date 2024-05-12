using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Astrum.Identity.Application.Contracts;
using Astrum.Identity.Application.ViewModels;
using Astrum.Identity.Models;
using Astrum.SharedLib.Application.Helpers;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Astrum.Identity.Infrastructure.Services
{
    public class IdentityJwtGenerator : IIdentityJwtGenerator
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IMapper _mapper;
        private readonly JwtManager _jwtManager;

        public IdentityJwtGenerator(IConfiguration config, IMapper mapper)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Authentication:JwtBearer:SecurityKey"]));
            _jwtManager = new JwtManager(_key);
            _mapper = mapper;
        }

        public string CreateToken(ApplicationUser user, IEnumerable<string> roles = null)
        {
            var userForm = _mapper.Map<TokenGenerationForm>(user);
            userForm.Roles = roles;
            return CreateToken(userForm);
        }

        public string CreateToken(TokenGenerationForm user)
        {
            var claims = new List<(string, string)>{
                (JwtRegisteredClaimNames.Sub, user.Id),
                (JwtRegisteredClaimNames.NameId, user.UserName),
                (JwtRegisteredClaimNames.Email, user.Email)
            };
            if (user.Roles != null)
                foreach (var role in user.Roles)
                    claims.Add(("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role));

            return _jwtManager.CreateJwtToken(claims);
        }

        public bool ValidateToken(string authToken)
        {
            return _jwtManager.ValidateToken(authToken);
        }
    }
}
