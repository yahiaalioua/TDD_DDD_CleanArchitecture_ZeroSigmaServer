using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.Common.Interfaces;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Infrastructure.Persistance.Repositories.IdentityAccess;

namespace ZeroSigma.Application.Authentication.Services.ProcessingServices.NewSessionProcessingServices
{
    public class NewSessionProcessingService : INewSessionProcessingService
    {
        private readonly IJwtTokenProcessingService _JwtTokenProcessingService;
        private readonly IIdentityAccessRepository _identityAccessRepository;
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        public NewSessionProcessingService(
            IJwtTokenProcessingService JwtTokenProcessingService,
            IAccessTokenProvider accessTokenProvider,
            IRefreshTokenProvider refreshTokenProvider
,
            IIdentityAccessRepository identityAccessRepository)
        {
            _JwtTokenProcessingService = JwtTokenProcessingService;
            _accessTokenProvider = accessTokenProvider;
            _refreshTokenProvider = refreshTokenProvider;
            _identityAccessRepository = identityAccessRepository;
        }

        public void RevokeRefreshTokenAndAddToBlackList(UserRefreshToken storedRefreshToken,UserAccessBlackList userAccessBlackList)
        {
             userAccessBlackList.RevokedRefreshTokens.Add(storedRefreshToken.RefreshToken);
            _identityAccessRepository.AddUserAccessBlacklistAsync(userAccessBlackList);
        }

        public void RemoveOldRefreshToken(UserAccessBlackList userAccessBlackList)
        {
            if (userAccessBlackList.RevokedRefreshTokens.Count > 30)
            {
                userAccessBlackList.RevokedRefreshTokens.RemoveAt(-1);
            }
        }
        public async Task<string> RevokeAndRotateRefreshToken(UserRefreshToken storedRefreshToken,Guid id,string email, UserAccessBlackList userAccessBlackList)
        {
            RevokeRefreshTokenAndAddToBlackList(storedRefreshToken,userAccessBlackList);
            RemoveOldRefreshToken(userAccessBlackList);
            var newRefreshToken = _refreshTokenProvider.GenerateRefreshToken(id, email);
            await _identityAccessRepository.DeleteRefreshTokenByIdAsync(storedRefreshToken.Id);
            var newUserSession = UserRefreshToken.Create(storedRefreshToken.userID, newRefreshToken, _JwtTokenProcessingService.DecodeJwt(newRefreshToken).ValidFrom, _JwtTokenProcessingService.DecodeJwt(newRefreshToken).ValidTo);
            await _identityAccessRepository.AddUserRefreshTokenAsync(newUserSession);
            return newRefreshToken;
        }
        public async Task<Result<NewSessionResponse>> ProcessSuccessNewSessionValidation(UserRefreshToken userRefreshToken,ClaimsPrincipal validatedToken,UserAccessBlackList userAccessBlackList)
        {
            Guid userId = Guid.Parse(_JwtTokenProcessingService.DecodeJwt(userRefreshToken.RefreshToken).Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value);
            string userEmail = _JwtTokenProcessingService.DecodeJwt(userRefreshToken.RefreshToken).Claims.Single(x => x.Type == ClaimTypes.Email).Value;
            string userFullName = validatedToken.Claims.Single(x => x.Type == ClaimTypes.Name).Value;
            var newRefreshToken=await RevokeAndRotateRefreshToken(userRefreshToken, userId, userEmail,userAccessBlackList);
            var newAccessToken = _accessTokenProvider.GenerateAccessToken(userId, userFullName, userEmail);
            NewSessionResponse response = new(newAccessToken, newRefreshToken);

            return new SuccessResult<NewSessionResponse>(response);
        }
        public async Task<Result<NewSessionResponse>> ProcessNewSession(NewSessionRequest request)
        {           
            UserID userId = UserID.Create(Guid.Parse(_JwtTokenProcessingService.DecodeJwt(request.refreshToken).Claims.Single(x => x.Type == ClaimTypes.NameIdentifier).Value));
            var userAccess = await _identityAccessRepository.GetUserAccessByUserId(userId);
            var storedNewSessionAccess= await _identityAccessRepository.GetUserRefreshTokenByIdAsync(userAccess.RefreshTokenID);
            var userAccessBlackList = await _identityAccessRepository.GetUserAccessBlacklistAsync(storedNewSessionAccess.Id);
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
            return await ProcessSuccessNewSessionValidation(storedNewSessionAccess, validatedToken, userAccessBlackList!);

        }
    }
}
