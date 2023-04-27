using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Common.Authentication;

namespace ZeroSigma.Infrastructure.Authentication.AccessToken
{
    public class AccessTokenProvider : IAccessTokenProvider
    {
        public readonly IJwtGenerator _jwtGenerator;
        private readonly AccessTokenOptions _options;

        public AccessTokenProvider(IJwtGenerator jwtGenerator, IOptions<AccessTokenOptions> options)
        {
            _jwtGenerator = jwtGenerator;
            _options = options.Value;
        }

        public string GenerateAccessToken(Guid Id, string name, string email)
        {
            
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, name),
                new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                new Claim(ClaimTypes.Email, email),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
            };
            
            var accessToken = _jwtGenerator.GenerateJwt(
                _options.AccessTokenSecretKey,
                _options.Issuer,
                _options.Audience,
                _options.AccessTokenExpirationMinutes,
                claims
                );
            return accessToken;
        }
    }
}
