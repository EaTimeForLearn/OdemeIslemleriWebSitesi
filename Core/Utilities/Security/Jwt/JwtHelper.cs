using Core.Entities.Concrete;
using Core.Extensions;
using Core.Utilities.Security.Encryption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Core.Utilities.Security.Jwt
{
    public class JwtHelper : ITokenHelper
    {
        TokenOptions _tokenOptions;
        public JwtHelper(IConfiguration configuration)
        {
            _tokenOptions = configuration.GetSection("TokenOptions").Get<TokenOptions>();
        }

        public Token CreateToken(User user, List<OperationClaim> operationClaims, string DBName) {
            return new Token
            {
                AccessToken = GetTokenString(user, operationClaims, DBName, _tokenOptions.AccessTokenSecurityKey, DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration)),
                RefreshToken = GetTokenString(user, operationClaims, DBName, _tokenOptions.RefreshTokenSecurityKey, DateTime.Now.AddDays(_tokenOptions.RefreshTokenExpiration)),
            };
        }

        private string GetTokenString(User user, List<OperationClaim> operationClaims, string DBName, string securityKey, DateTime expireDate)
        {
            var secKey = SecurityKeyHelper.CreateSecurityKey(securityKey);
            var SigningCredentials = SigningCredentialsHelper.CreateSigningCredentials(secKey);
            var jwt = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                expires: expireDate,
                notBefore: DateTime.Now,
                claims: SetClaims(user, operationClaims, DBName),
                signingCredentials: SigningCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            return jwtSecurityTokenHandler.WriteToken(jwt);
            
        }

        
        private IEnumerable<Claim> SetClaims(User user, List<OperationClaim> operationClaims, string DBName) 
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddEmail(user.Email);
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(operationClaims.Select(c => c.Name).ToArray());
            claims.AddDBName(DBName);
            return claims;
        }
    }
}
