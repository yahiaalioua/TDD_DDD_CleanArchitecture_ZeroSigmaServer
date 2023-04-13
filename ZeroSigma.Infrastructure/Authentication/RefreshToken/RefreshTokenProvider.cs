using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Common.Authentication;

namespace ZeroSigma.Infrastructure.Authentication.RefreshToken
{
    public class RefreshTokenProvider : IRefreshTokenProvider
    {
        private readonly IJwtGenerator _jwtGenerator;
        private readonly RefreshTokenOptions _refreshTokenOptions;
        public RefreshTokenProvider(IJwtGenerator jwtGenerator,IOptions<RefreshTokenOptions> refreshTokenOptions)
        {
            _jwtGenerator = jwtGenerator;
            _refreshTokenOptions = refreshTokenOptions.Value;
        }

        public string GenerateRefreshToken(Guid id, string email)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, email),
                new Claim(ClaimTypes.NameIdentifier,id.ToString())
            };
            var RefreshToken = _jwtGenerator.GenerateJwt(
                _refreshTokenOptions.RefreshTokenSecretKey,
                _refreshTokenOptions.Issuer,
                _refreshTokenOptions.Audience,
                _refreshTokenOptions.RefreshTokenExpirationMinutes,
                claims
                );
            return RefreshToken;
        }
    }
}
