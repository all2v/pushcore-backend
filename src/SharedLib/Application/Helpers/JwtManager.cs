using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;

namespace Astrum.SharedLib.Application.Helpers
{
    public class JwtManager
    {
        private readonly SymmetricSecurityKey _key;

        public JwtManager(SymmetricSecurityKey securityKey) 
        {
            _key = securityKey;
        }

        public string CreateJwtToken(List<(string key, string value)> payload)
        {
            var claims = payload.Select(p => new Claim(p.key, p.value)).ToList();

            var credentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(7),
                SigningCredentials = credentials
            };
            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public bool ValidateToken(string authToken)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();
                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out var validatedToken);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static IEnumerable<Claim> GetJwtTokenClaims(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jsonToken = handler.ReadToken(token);
                var tokenS = jsonToken as JwtSecurityToken;
                return tokenS.Claims;
            }
            catch
            {
                return null;
            }
        }

        public static Guid? GetUserIdFromRequest(HttpRequest request)
        {
            var tokenExists = request.Headers.TryGetValue("Authorization", out var token);
            if (!tokenExists)
                return null;
            var bearerToken = token.ToString()?.Split(" ")?.LastOrDefault();
            var claims = JwtManager.GetJwtTokenClaims(bearerToken);
            if (claims == null ||
                !Guid.TryParse(claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value,
                out var userId))
                return null;
            return userId;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                ValidIssuer = "Sample",
                ValidAudience = "Sample",
                IssuerSigningKey = _key // The same key as the one that generate the token
            };
        }
    }
}
