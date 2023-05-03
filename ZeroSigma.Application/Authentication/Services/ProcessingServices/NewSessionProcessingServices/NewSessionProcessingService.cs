using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.Common.Interfaces;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;

namespace ZeroSigma.Application.Authentication.Services.ProcessingServices.NewSessionProcessingServices
{
    public class NewSessionProcessingService : INewSessionProcessingService
    {
        private readonly IJwtTokenProcessingService _JwtTokenProcessingService;
        private readonly IUserAccessRepository _userAccessRepository;
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        public NewSessionProcessingService(
            IJwtTokenProcessingService JwtTokenProcessingService,
            IUserAccessRepository userAccessRepository,
            IAccessTokenProvider accessTokenProvider,
            IRefreshTokenProvider refreshTokenProvider
            )
        {
            _JwtTokenProcessingService = JwtTokenProcessingService;
            _userAccessRepository = userAccessRepository;
            _accessTokenProvider = accessTokenProvider;
            _refreshTokenProvider = refreshTokenProvider;
        }

        public void RevokeRefreshTokenAndAddToBlackList(UserRefreshToken storedRefreshToken,UserAccessBlackList userAccessBlackList)
        {
             userAccessBlackList.RevokedRefreshTokens.Add(storedRefreshToken.RefreshToken);
            _userAccessRepository.AddUserAccessBlacklist(userAccessBlackList);
        }

        public void RemoveOldRefreshToken(UserAccessBlackList userAccessBlackList)
        {
            if (userAccessBlackList.RevokedRefreshTokens.Count > 30)
            {
                userAccessBlackList.RevokedRefreshTokens.RemoveAt(-1);
            }
        }
        public string RevokeAndRotateRefreshToken(UserRefreshToken storedRefreshToken,Guid id,string email, UserAccessBlackList userAccessBlackList)
        {
            RevokeRefreshTokenAndAddToBlackList(storedRefreshToken,userAccessBlackList);
            RemoveOldRefreshToken(userAccessBlackList);
            var newRefreshToken = _refreshTokenProvider.GenerateRefreshToken(id, email);
            _userAccessRepository.DeleteRefreshToken(storedRefreshToken.Id);
            var newUserSession = UserRefreshToken.Create(storedRefreshToken.userID, newRefreshToken, _JwtTokenProcessingService.DecodeJwt(newRefreshToken).ValidFrom, _JwtTokenProcessingService.DecodeJwt(newRefreshToken).ValidTo);
            _userAccessRepository.AddUserRefreshToken(newUserSession);
            return newRefreshToken;
        }
        public Result<NewSessionResponse> ProcessSuccessNewSessionValidation(UserRefreshToken userRefreshToken,ClaimsPrincipal validatedToken,UserAccessBlackList userAccessBlackList)
        {
            Guid userId = Guid.Parse(_JwtTokenProcessingService.DecodeJwt(userRefreshToken.RefreshToken).Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            string userEmail = _JwtTokenProcessingService.DecodeJwt(userRefreshToken.RefreshToken).Claims.Single(x => x.Type == ClaimTypes.Email).Value;
            string userFullName = validatedToken.Claims.Single(x => x.Type == ClaimTypes.Name).Value;
            var newRefreshToken=RevokeAndRotateRefreshToken(userRefreshToken, userId, userEmail,userAccessBlackList);
            var newAccessToken = _accessTokenProvider.GenerateAccessToken(userId, userFullName, userEmail);
            NewSessionResponse response = new(newAccessToken, newRefreshToken);

            return new SuccessResult<NewSessionResponse>(response);
        }
        public Result<NewSessionResponse> ProcessNewSession(NewSessionRequest request)
        {           
            Guid userId = Guid.Parse(_JwtTokenProcessingService.DecodeJwt(request.refreshToken).Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            var storedNewSessionAccess= _userAccessRepository.GetUserRefreshTokenByUserID(userId);
            var userAccessBlackList = _userAccessRepository.GetUserAccessBlackList(storedNewSessionAccess.Id);
            if (userAccessBlackList is not null)
            {
                if (userAccessBlackList.RevokedRefreshTokens.Contains(request.refreshToken))
                {
                    return new InvalidResult<NewSessionResponse>(NewSessionLogicalValidationErrors.TokenReusedError);
                };
            }
            var validatedToken = _JwtTokenProcessingService.Validate(request.accessToken);
            if (validatedToken is null)
            {
                return new InvalidResult<NewSessionResponse>(NewSessionLogicalValidationErrors.InvalidTokenError);
            }
            var userIdFromAccessToken = validatedToken.Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (storedNewSessionAccess is null)
            {
                return new NotFoundResults<NewSessionResponse>(NewSessionLogicalValidationErrors.TokenNotFoundError);
            }
            if (storedNewSessionAccess.ExpiryDate < DateTime.UtcNow)
            {
                return new InvalidResult<NewSessionResponse>(NewSessionLogicalValidationErrors.TokenExpiredError);
            }
            var userIdFromRefreshToken = _JwtTokenProcessingService.DecodeJwt(storedNewSessionAccess.RefreshToken).Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value;
            if (userIdFromRefreshToken != userIdFromAccessToken)
            {
                return new InvalidResult<NewSessionResponse>(NewSessionLogicalValidationErrors.InvalidTokenError);
            }
            return ProcessSuccessNewSessionValidation(storedNewSessionAccess, validatedToken, userAccessBlackList!);

        }
    }
}
