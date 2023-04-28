using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Authentication.Queries.NewSession;
using ZeroSigma.Application.Authentication.Services.ProcessingServices.NewSessionProcessingServices;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.Common.Interfaces;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;

namespace ZeroSigma.Application.Authentication.Queries
{
    public class NewSessionQueryTests
    {
        private readonly INewSessionProcessingService _sessionProcessingService;
        private readonly Mock<IJwtTokenProcessingService> _jwtTokenProcessingServiceMock;
        private readonly Mock<IUserAccessRepository> _userAccessRepositoryMock;
        private readonly Mock<IAccessTokenProvider> _accessTokenProviderMock;
        public NewSessionQueryTests()
        {
            _jwtTokenProcessingServiceMock = new Mock<IJwtTokenProcessingService>();
            _accessTokenProviderMock = new Mock<IAccessTokenProvider>();
            _userAccessRepositoryMock = new Mock<IUserAccessRepository>();
            _sessionProcessingService = new NewSessionProcessingService(
                _jwtTokenProcessingServiceMock.Object,
                _userAccessRepositoryMock.Object,
                _accessTokenProviderMock.Object
                );
        }

        [Fact]
        public async Task ShouldReturnInvalidTokenErrorWhenTokenWhenValidatedTokenIsNull()
        {
            //arrange
            var accessToken = "FakeAccessToken836sgsra62,s.nxcbz5a3%4fs";
            var refreshToken = "FakeRefreshToken836sgsra62,s.nxcbz5a3%4fs";
            var query = new NewSessionQuery(accessToken,refreshToken);
            NewSessionRequest newSessionRequest = new(accessToken, refreshToken);
            var handler=new NewSessionQueryHandler(_sessionProcessingService);
            _jwtTokenProcessingServiceMock.Setup(x => x.Validate(It.IsAny<string>())).Returns<ClaimsPrincipal>(null);
            _accessTokenProviderMock.Setup(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(It.IsAny<string>());
            _userAccessRepositoryMock.Setup(x => x.GetUserRefreshToken(It.IsAny<string>())).Returns(It.IsAny<UserRefreshToken>());

            //act
            Result<string> result = await handler.Handle(query, default);
            //assert
            _jwtTokenProcessingServiceMock.Verify(x=>x.Validate(It.IsAny<string>()), Times.Once());
            _accessTokenProviderMock.Verify(x=>x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(),
                It.IsAny<string>()), Times.Never());
            _userAccessRepositoryMock.Verify(x => x.GetUserRefreshToken(It.IsAny<string>()),Times.Never());
            Assert.Equal(result.CustomProblemDetails, NewSessionLogicalValidationErrors.InvalidTokenError);
        }
    }
}
