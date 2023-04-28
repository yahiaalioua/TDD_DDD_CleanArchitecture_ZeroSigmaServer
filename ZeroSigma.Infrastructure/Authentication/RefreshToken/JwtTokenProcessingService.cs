﻿using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Common.Interfaces;

namespace ZeroSigma.Infrastructure.Authentication.RefreshToken
{
    public class JwtTokenProcessingService: IJwtTokenProcessingService
    {
        private readonly IJwtTokenValidation _tokenValidation;

        public JwtTokenProcessingService(IJwtTokenValidation parameters)
        {
            _tokenValidation = parameters;
        }
        public ClaimsPrincipal Validate(string refreshToken)
        {
            var tokenValidationParameters = _tokenValidation.Parameters();
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(refreshToken, tokenValidationParameters, out var validatedToken);
                if (!_tokenValidation.ValidateJwtSecurityAlgorith(validatedToken))
                {
                    return null;
                }
                return principal;
            }
            catch (Exception)
            {

                return null;
            };

        }
    }
}