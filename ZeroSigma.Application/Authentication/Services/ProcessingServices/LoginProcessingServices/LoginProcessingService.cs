using System.IdentityModel.Tokens.Jwt;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.Common.Interfaces;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.UserAccess;
using ZeroSigma.Infrastructure.Persistance.Repositories.IdentityAccess;

namespace ZeroSigma.Application.Authentication.Services.ProcessingServices.AuthenticationProcessingServices
{
    public class LoginProcessingService : ILoginProcessingService
    {
        private readonly IIdentityAccessRepository _identityAccessRepository;
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        private readonly IUnitOfWork _unitOfWork;

        public LoginProcessingService(
            IAccessTokenProvider accessTokenProvider,
            IRefreshTokenProvider refreshTokenProvider,
            IIdentityAccessRepository identityAccessRepository,
            IUnitOfWork unitOfWork)

        {
            _accessTokenProvider = accessTokenProvider;
            _refreshTokenProvider = refreshTokenProvider;
            _identityAccessRepository = identityAccessRepository;
            _unitOfWork = unitOfWork;
        }
        public JwtSecurityToken DecodeJwt(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var decodedJwt = handler.ReadJwtToken(token);
            return decodedJwt;
        }
        public DateTime GetTokenExpiryDate(string token)
        {
            return DecodeJwt(token).ValidTo;
        }
        public DateTime GetTokenIssueDate(string token)
        {
            return DecodeJwt(token).ValidFrom;
        }
        public async Task PersistIdentity(
            User user, UserAccessToken userAccessToken,
            UserRefreshToken userRefreshToken,UserAccessBlackList userAccessBlackList,UserAccess userAccess
            )
        {
            if (await _identityAccessRepository.GetUserAccessByUserId(user.Id) is null)
            {
                await _identityAccessRepository.AddUserAccessTokenAsync(userAccessToken);
                await _identityAccessRepository.AddUserRefreshTokenAsync(userRefreshToken);
                await _identityAccessRepository.AddUserAccessBlacklistAsync(userAccessBlackList);
                await _identityAccessRepository.AddUserAccessAsync(userAccess);
                await _unitOfWork.SaveChangesAsync();

            }
            if (await _identityAccessRepository.GetUserAccessByUserId(user.Id) is not null)
            {
                await _identityAccessRepository.UpdateUserAccessToken(userAccessToken);
                await _identityAccessRepository.UpdateUserRefreshToken(userRefreshToken);
                await _unitOfWork.SaveChangesAsync();

            }
        }
        public async Task<ProcessedAuthenticationResponse> ProcessAuthentication(User user)
        {
            string accessToken = _accessTokenProvider.GenerateAccessToken(user.Id.Value, user.FullName.Value, user.Email.Value);
            var refreshToken = _refreshTokenProvider.GenerateRefreshToken(user.Id.Value, user.Email.Value);
            DateTime accesstokenIssuedDate = GetTokenIssueDate(accessToken);
            DateTime accesstokenExpirydate = GetTokenExpiryDate(accessToken);
            DateTime refreshTokenIssuedDate = GetTokenIssueDate(refreshToken);
            DateTime refreshTokenExpirydate = GetTokenExpiryDate(refreshToken);
            UserAccessToken userAccessToken = UserAccessToken.Create(accessToken, accesstokenIssuedDate, accesstokenExpirydate);
            UserRefreshToken userRefreshToken = UserRefreshToken.Create(user.Id, refreshToken, refreshTokenIssuedDate, refreshTokenExpirydate);
            UserAccess userAccess = UserAccess.Create(user.Id, userAccessToken.Id, userRefreshToken.Id);
            UserAccessBlackList userAccessBlackList = UserAccessBlackList.Create(userRefreshToken.Id);
            await PersistIdentity(user, userAccessToken, userRefreshToken, userAccessBlackList, userAccess);
            
            return new ProcessedAuthenticationResponse() { AccessToken=accessToken,RefreshToken=refreshToken};
        }
    }
}
