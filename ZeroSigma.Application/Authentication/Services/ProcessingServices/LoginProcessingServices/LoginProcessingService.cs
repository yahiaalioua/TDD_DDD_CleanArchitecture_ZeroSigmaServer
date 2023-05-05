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
        private readonly IUnitOfWork _unitOfWork;

        public LoginProcessingService(
            IAccessTokenProvider accessTokenProvider,
            IRefreshTokenProvider refreshTokenProvider,
            IIdentityAccessRepository identityAccessRepository,
            IUnitOfWork unitOfWork)

        {
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
            User user,string accessToken,string refreshToken
            )
        {
            string newAccessToken = accessToken;
            string newRefreshToken = refreshToken;
            DateTime accesstokenIssuedDate = GetTokenIssueDate(accessToken);
            DateTime accesstokenExpirydate = GetTokenExpiryDate(accessToken);
            DateTime refreshTokenIssuedDate = GetTokenIssueDate(refreshToken);
            DateTime refreshTokenExpirydate = GetTokenExpiryDate(refreshToken);            
            if (await _identityAccessRepository.GetUserAccessByUserId(user.Id) is null)
            {
                UserAccessToken newUserAccessToken = UserAccessToken.Create(accessToken, accesstokenIssuedDate, accesstokenExpirydate);
                UserRefreshToken newUserRefreshToken = UserRefreshToken.Create(refreshToken, refreshTokenIssuedDate, refreshTokenExpirydate);
                UserAccess userAccess = UserAccess.Create(user.Id, newUserAccessToken.Id, newUserRefreshToken.Id);
                UserAccessBlackList userAccessBlackList = UserAccessBlackList.Create(newUserRefreshToken.Id);
                await _identityAccessRepository.AddUserAccessTokenAsync(newUserAccessToken);
                await _identityAccessRepository.AddUserRefreshTokenAsync(newUserRefreshToken);
                await _identityAccessRepository.AddUserAccessBlacklistAsync(userAccessBlackList);
                await _identityAccessRepository.AddUserAccessAsync(userAccess);
                await _unitOfWork.SaveChangesAsync();

            }
            if (await _identityAccessRepository.GetUserAccessByUserId(user.Id) is not null)
            {
                var identityUserAccess=await _identityAccessRepository.GetUserAccessByUserId(user.Id);
                var storedUserRefreshToken = await _identityAccessRepository.GetUserRefreshTokenByIdAsync(identityUserAccess?.RefreshTokenID!);
                var storedUserAccessToken= await _identityAccessRepository.GetUserAccessTokenByIdAsync(identityUserAccess?.AccessTokenID!);
                var updatedUserAccessToken = UserAccessToken.Create(accessToken, accesstokenIssuedDate, accesstokenExpirydate);
                var updatedUserRefreshToken=UserRefreshToken.Create(refreshToken,refreshTokenIssuedDate, refreshTokenExpirydate);
                await _identityAccessRepository.UpdateUserAccessToken(storedUserAccessToken.Id,updatedUserAccessToken);
                await _identityAccessRepository.UpdateUserRefreshToken(storedUserRefreshToken.Id,updatedUserRefreshToken);
                await _unitOfWork.SaveChangesAsync();
            }
        }
        public async Task ProcessAuthentication(User user,string accessToken,string refreshToken)
        {            
            await PersistIdentity(user,accessToken,refreshToken);
        }
    }
}
