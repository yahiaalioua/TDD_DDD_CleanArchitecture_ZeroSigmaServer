using Moq;
using ZeroSigma.Application.Authentication.Services.Encryption;
using ZeroSigma.Application.Authentication.Services.ProcessingServices;
using ZeroSigma.Application.Authentication.Services.ProcessingServices.AuthenticationProcessingServices;
using ZeroSigma.Application.Authentication.Services.ValidationServices.Login;
using ZeroSigma.Application.Authentication.Services.ValidationServices.SignUp;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;
using ZeroSigma.Domain.ValueObjects.User;

namespace ZeroSigma.Application.Authentication.Queries
{
    public class LoginQueryTests
    {

        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IEncryptionService> _encryptionServiceMock;
        private Mock<ILoginProcessingService> _loginProcessingServiceMock;
        private readonly User _mike;
        private readonly FullName _fullName;
        private readonly UserEmail _email;
        private readonly UserPassword _password;
        private readonly ILoginValidationService _loginValidationService;
        public LoginQueryTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _password = UserPassword.Create("UserPass2453..?").Data;
            _fullName = FullName.Create("mike").Data;
            _email = UserEmail.Create("mike@mail.com").Data;
            _mike = User.Create(_fullName, _email, _password);
            _encryptionServiceMock = new Mock<IEncryptionService>();
            _loginProcessingServiceMock = new Mock<ILoginProcessingService>();
            _loginValidationService = new LoginValidationService(
                _encryptionServiceMock.Object, _loginProcessingServiceMock.Object
                );
        }

        [Fact]
        public async Task Handle_ShouldReturnNonExistantEmailErrorWhenEmailIsNotFoundInDb()
        {
            // arrange
            var query = new LoginQuery(_mike.Email.Value, _mike.Password.Value);
            User? user = null;
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<UserEmail>())).ReturnsAsync(user);
           
            var handler = new LoginQueryHandler(
                _userRepositoryMock.Object,
                _loginValidationService
                );
            //act
            Result<AuthenticationResponse> result = await handler.Handle(query, default);
            //assert
            _userRepositoryMock.Verify(r=>r.GetByEmailAsync(It.IsAny<UserEmail>()), Times.Once());
            Assert.True(result.CustomProblemDetails == LoginLogicalValidationErrors.NonExistentEmailError);
        }
        [Fact]
        public async Task Handle_ShouldReturnInvalidPasswordErrorWhenPasswordIsWrong()
        {
            // arrange
            var query = new LoginQuery(_mike.Email.Value, "wrongPassword");
            User user = _mike;            
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(_mike.Email)).ReturnsAsync(user);
            var handler = new LoginQueryHandler(
                _userRepositoryMock.Object,
                _loginValidationService
                );
            //act
            Result<AuthenticationResponse> result = await handler.Handle(query, default);
            //assert
            _userRepositoryMock.Verify(r => r.GetByEmailAsync(_mike.Email), Times.Once());            
            Assert.True(result.CustomProblemDetails == LoginLogicalValidationErrors.InvalidPasswordError);
        }
        [Fact]
        public async Task Handle_ShouldReturnSuccessAuthenticationResponseWhenEmailAndPasswordIsCorrect()
        {
            // arrange
            var processedAuthResponse = new ProcessedAuthenticationResponse() {
                AccessToken = "accessToken", RefreshToken = "refreshToken" 
            };
            var query = new LoginQuery(_mike.Email.Value,_mike.Password.Value);
            User user = _mike;
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(_mike.Email)).ReturnsAsync(user);
            _loginProcessingServiceMock.Setup(x => x.ProcessAuthentication(_mike)).ReturnsAsync(processedAuthResponse);
            _encryptionServiceMock.Setup(x => x.VerifyPassword(query.Password, _mike.Password.Value)).Returns(true);
            var handler = new LoginQueryHandler(
                _userRepositoryMock.Object,
                _loginValidationService
                );
            //act
            Result<AuthenticationResponse> result = await handler.Handle(query, default);
            //assert
            _userRepositoryMock.Verify(r => r.GetByEmailAsync(_mike.Email), Times.Once());
            _loginProcessingServiceMock.Verify(x => x.ProcessAuthentication(_mike));
            _encryptionServiceMock.Verify(x => x.VerifyPassword(query.Password, _mike.Password.Value));
            Assert.True(result.ResultType == ResultType.Ok);            
        }
    }
}
