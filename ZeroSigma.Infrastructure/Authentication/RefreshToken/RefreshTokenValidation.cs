using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Infrastructure.Authentication.AccessToken;

namespace ZeroSigma.Infrastructure.Authentication.RefreshToken
{
    public class RefreshTokenValidation : IRefreshTokenValidation
    {
        private readonly AccessTokenOptions _options;

        public RefreshTokenValidation(IOptions<AccessTokenOptions> options)
        {
            _options = options.Value;
        }

        public bool ValidateJwtSecurityAlgorith(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken)&&
                jwtSecurityToken.Header.Alg.Equals(value:SecurityAlgorithms.HmacSha256,StringComparison.InvariantCultureIgnoreCase);
        }

        public TokenValidationParameters Parameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _options.Issuer,
                ValidAudience = _options.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(_options.AccessTokenSecretKey))
            };

        }
    }
}
