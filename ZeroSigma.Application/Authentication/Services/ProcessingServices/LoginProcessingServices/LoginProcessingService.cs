using System.IdentityModel.Tokens.Jwt;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.Common.Interfaces;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;

namespace ZeroSigma.Application.Authentication.Services.ProcessingServices.AuthenticationProcessingServices
{
    public class LoginProcessingService : ILoginProcessingService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserAccessRepository _userAccessRepository;
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;

        public LoginProcessingService(
            IUserRepository userRepository,
            IUserAccessRepository userAccessRepository,
            IAccessTokenProvider accessTokenProvider,
            IRefreshTokenProvider refreshTokenProvider)

        {
            _userRepository = userRepository;
            _userAccessRepository = userAccessRepository;
            _accessTokenProvider = accessTokenProvider;
            _refreshTokenProvider = refreshTokenProvider;
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
        public async Task<ProcessedAuthenticationResponse> ProcessAuthentication(User user)
        {
            string accessToken = _accessTokenProvider.GenerateAccessToken(user.Id.Value, user.FullName.Value, user.Email.Value);
            var refreshToken = _refreshTokenProvider.GenerateRefreshToken(user.Id.Value, user.Email.Value);
            DateTime accesstokenIssuedDate = GetTokenIssueDate(accessToken);
            DateTime accesstokenExpirydate = GetTokenExpiryDate(accessToken);
            DateTime refreshTokenIssuedDate = GetTokenIssueDate(refreshToken);
            DateTime refreshTokenExpirydate = GetTokenExpiryDate(refreshToken);
            if (_userAccessRepository.GetUserAccessById(user.Id) is null)
            {
                UserAccessToken userAccessToken = UserAccessToken.Create(accessToken, accesstokenIssuedDate,accesstokenExpirydate);
                UserRefreshToken userRefreshToken=UserRefreshToken.Create(user.Id,refreshToken, refreshTokenIssuedDate,refreshTokenExpirydate);
                UserAccess userAccess = UserAccess.Create(user.Id, userAccessToken.Id,userRefreshToken.Id);
                UserAccessBlackList userAccessBlackList = UserAccessBlackList.Create(userRefreshToken.Id);
                _userAccessRepository.AddUserAccessBlacklist(userAccessBlackList);
                _userAccessRepository.AddUserAccess(userAccess);
                _userAccessRepository.AddUserAccessToken(userAccessToken);
                _userAccessRepository.AddUserRefreshToken(userRefreshToken);
                await _userRepository.AddUserAsync(user);
                
            }
            return new ProcessedAuthenticationResponse() { AccessToken=accessToken,RefreshToken=refreshToken};
        }
    }
}
