using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.Common.Interfaces;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;

namespace ZeroSigma.Application.Authentication.Services.ProcessingServices.NewSessionProcessingServices
{
    public class NewSessionProcessingService : INewSessionProcessingService
    {
        private readonly IJwtTokenProcessingService _refreshTokenProcessingService;
        private readonly IUserAccessRepository _userAccessRepository;
        private readonly IAccessTokenProvider _accessTokenProvider;

        public NewSessionProcessingService(
            IJwtTokenProcessingService refreshTokenProcessingService,
            IUserAccessRepository userAccessRepository,
            IAccessTokenProvider accessTokenProvider)
        {
            _refreshTokenProcessingService = refreshTokenProcessingService;
            _userAccessRepository = userAccessRepository;
            _accessTokenProvider = accessTokenProvider;
        }
        public JwtSecurityToken DecodeJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedJwt = handler.ReadJwtToken(token);
            return decodedJwt;
        }
        public Result<string> ProcessNewSession(NewSessionRequest request)
        {
            var validatedToken = _refreshTokenProcessingService.Validate(request.accessToken);
            if (validatedToken == null)
            {
                return new InvalidResult<string>(NewSessionLogicalValidationErrors.InvalidTokenError);
            }
            var userIdFromAccessToken = validatedToken.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var storedRefreshToken = _userAccessRepository.GetUserRefreshToken(request.refreshToken);
            if (storedRefreshToken == null)
            {
                return new NotFoundResults<string>(NewSessionLogicalValidationErrors.TokenNotFoundError);
            }
            if (storedRefreshToken.ExpiryDate < DateTime.UtcNow)
            {
                return new InvalidResult<string>(NewSessionLogicalValidationErrors.TokenExpiredError);
            }
            var userIdFromRefreshToken = DecodeJwt(storedRefreshToken.RefreshToken).Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (userIdFromRefreshToken != userIdFromAccessToken)
            {
                return new InvalidResult<string>(NewSessionLogicalValidationErrors.InvalidTokenError);
            }
            Guid userId = Guid.Parse(DecodeJwt(storedRefreshToken.RefreshToken).Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            string userEmail = DecodeJwt(storedRefreshToken.RefreshToken).Claims.Single(x => x.Type == ClaimTypes.Email).Value;
            string userFullName = validatedToken.Claims.Single(x => x.Type == ClaimTypes.Name).Value;
            return new SuccessResult<string>(_accessTokenProvider.GenerateAccessToken(userId, userFullName,userEmail));

        }
    }
}
