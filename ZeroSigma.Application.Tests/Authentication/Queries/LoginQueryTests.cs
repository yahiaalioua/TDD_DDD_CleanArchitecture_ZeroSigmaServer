using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Authentication.Commands;
using ZeroSigma.Application.Authentication.Services.ValidationServices.Login;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Errors;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;

namespace ZeroSigma.Application.Authentication.Queries
{
    public class LoginQueryTests
    {

        private Mock<IUserRepository> _userRepositoryMock;
        private Mock<IAccessTokenProvider> _accessTokenProviderMock;
        private Mock<IRefreshTokenProvider> _refreshTokenProviderMock;
        private readonly User _mike;
        public LoginQueryTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mike = new User()
            {
                FullName = "Mike",
                Email = "mike@mail.com",
                Password = "passRandom",
                AccessToken = "accessToken",
                RefreshToken = "refreshToken"
            };
            _accessTokenProviderMock= new Mock<IAccessTokenProvider>();
            _refreshTokenProviderMock= new Mock<IRefreshTokenProvider>();
        }

        [Fact]
        public async Task Handle_ShouldReturnNonExistantEmailErrorWhenEmailIsNotFoundInDb()
        {
            // arrange
            var query = new LoginQuery(_mike.Email, _mike.Password);
            User? user = null;
            _userRepositoryMock.Setup(r => r.GetByEmail(query.Email)).Returns(user);
            ILoginValidationService loginValidationService = new LoginValidationService();

            var handler = new LoginQueryHandler(
                _accessTokenProviderMock.Object,
                _refreshTokenProviderMock.Object,
                loginValidationService,
                _userRepositoryMock.Object 
                );
            //act
            Result<AuthenticationResponse> result = await handler.Handle(query, default);
            //assert
            _userRepositoryMock.Verify(r=>r.GetByEmail(query.Email), Times.Once());
            Assert.True(result.CustomProblemDetails == LoginLogicalValidationErrors.NonExistentEmailError);
        }
        [Fact]
        public async Task Handle_ShouldReturnInvalidPasswordErrorWhenPasswordIsWrong()
        {
            // arrange
            var query = new LoginQuery(_mike.Email, "wrongPassword");
            User user = _mike;            
            _userRepositoryMock.Setup(r => r.GetByEmail(query.Email)).Returns(user);
            ILoginValidationService loginValidationService = new LoginValidationService();

            var handler = new LoginQueryHandler(
                _accessTokenProviderMock.Object,
                _refreshTokenProviderMock.Object,
                loginValidationService,
                _userRepositoryMock.Object
                );
            //act
            Result<AuthenticationResponse> result = await handler.Handle(query, default);
            //assert
            _userRepositoryMock.Verify(r => r.GetByEmail(query.Email), Times.Once());            
            Assert.True(result.CustomProblemDetails == LoginLogicalValidationErrors.InvalidPasswordError);
        }
        [Fact]
        public async Task Handle_ShouldReturnSuccessAuthenticationResponseWhenEmailAndPasswordIsCorrect()
        {
            // arrange
            var query = new LoginQuery(_mike.Email,_mike.Password);
            User user = _mike;
            _userRepositoryMock.Setup(r => r.GetByEmail(query.Email)).Returns(user);
            ILoginValidationService loginValidationService = new LoginValidationService();

            var handler = new LoginQueryHandler(
                _accessTokenProviderMock.Object,
                _refreshTokenProviderMock.Object,
                loginValidationService,
                _userRepositoryMock.Object
                );
            //act
            Result<AuthenticationResponse> result = await handler.Handle(query, default);
            //assert
            _userRepositoryMock.Verify(r => r.GetByEmail(query.Email), Times.Once());
            Assert.True(result.ResultType == ResultType.Ok);            
        }
    }
}
