using ZeroSigma.Application.Authentication.Services.Encryption;
using ZeroSigma.Application.Authentication.Services.ProcessingServices.AuthenticationProcessingServices;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;

namespace ZeroSigma.Application.Authentication.Services.ValidationServices.Login
{
    public class LoginValidationService:ILoginValidationService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly ILoginProcessingService _loginProcessingService;
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;

        public LoginValidationService(
            IEncryptionService encryptionService,
            ILoginProcessingService loginProcessingService,
            IAccessTokenProvider accessTokenProvider,
            IRefreshTokenProvider refreshTokenProvider
            )
        {
            _encryptionService = encryptionService;
            _loginProcessingService = loginProcessingService;
            _accessTokenProvider = accessTokenProvider;
            _refreshTokenProvider = refreshTokenProvider;
        }

        public async Task<Result<AuthenticationResponse>> ValidateUser(User? user, string password)
        {
            if(user is null)
            {
                return new NotFoundResults<AuthenticationResponse>(LoginLogicalValidationErrors.NonExistentEmailError);
            }
            if(!_encryptionService.VerifyPassword(password,user.Password.Value))
            {
                return new InvalidResult<AuthenticationResponse>(LoginLogicalValidationErrors.InvalidPasswordError);
            }
            string accessToken = _accessTokenProvider.GenerateAccessToken(user.Id.Value, user.FullName.Value, user.Email.Value);
            string refreshToken = _refreshTokenProvider.GenerateRefreshToken(user.Id.Value, user.Email.Value);
            await _loginProcessingService.ProcessAuthentication(user,accessToken,refreshToken);
            AuthenticationResponse authenticationResponse = new()
            {
                Id = user.Id.Value,
                FullName = user.FullName.Value,
                Email = user.Email.Value,
                AccessToken = accessToken,
                RefreshToken=refreshToken,
            };
            return new SuccessResult<AuthenticationResponse>(authenticationResponse);
            
        }
    }
}
