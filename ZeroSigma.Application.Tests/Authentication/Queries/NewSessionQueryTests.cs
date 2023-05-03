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
    /*public class NewSessionQueryTests
    {
        private readonly INewSessionProcessingService _sessionProcessingService;
        private readonly Mock<IJwtTokenProcessingService> _jwtTokenProcessingServiceMock;
        private readonly Mock<IUserAccessRepository> _userAccessRepositoryMock;
        private readonly Mock<IAccessTokenProvider> _accessTokenProviderMock;
        private readonly List<Claim> _claims;
        private readonly ClaimsIdentity _identity;
        private readonly ClaimsPrincipal _claimsPrincipal;
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
            _claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, "username"),
                new Claim(ClaimTypes.NameIdentifier, "49580553-60f4-4a58-87da-da874643cdf5"),
                new Claim("name", "John Doe"),

            };
            _identity=new ClaimsIdentity(_claims, "TestAuthType");
            _claimsPrincipal = new ClaimsPrincipal(_identity);
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
        [Fact]
        public async Task ShouldReturnTokenNotFoundErrorWhenStoredRefreshTokenIsNull()
        {
            //arrange
            var accessToken = "FakeAccessToken836sgsra62,s.nxcbz5a3%4fs";
            var refreshToken = "FakeRefreshToken836sgsra62,s.nxcbz5a3%4fs";
            var query = new NewSessionQuery(accessToken, refreshToken);
            NewSessionRequest newSessionRequest = new(accessToken, refreshToken);
            var handler = new NewSessionQueryHandler(_sessionProcessingService);
            _jwtTokenProcessingServiceMock.Setup(x => x.Validate(It.IsAny<string>())).Returns(_claimsPrincipal);
            _accessTokenProviderMock.Setup(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(It.IsAny<string>());
            _userAccessRepositoryMock.Setup(x => x.GetUserRefreshToken(It.IsAny<string>())).Returns<UserRefreshToken>(null);

            //act
            Result<string> result = await handler.Handle(query, default);
            //assert
            _jwtTokenProcessingServiceMock.Verify(x => x.Validate(It.IsAny<string>()), Times.Once());
            _accessTokenProviderMock.Verify(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(),
                It.IsAny<string>()), Times.Never());
            _userAccessRepositoryMock.Verify(x => x.GetUserRefreshToken(It.IsAny<string>()), Times.Once());
            Assert.Equal(result.CustomProblemDetails, NewSessionLogicalValidationErrors.TokenNotFoundError);
        }
        [Fact]
        public async Task ShouldReturnTokenExpiredErrorWhenStoredRefreshTokenExpiryDateIsLessThanCurrentDate()
        {
            //arrange
            var accessToken = "FakeAccessToken836sgsra62,s.nxcbz5a3%4fs";
            var refreshToken = "FakeRefreshToken836sgsra62,s.nxcbz5a3%4fs";
            DateTime refreshTokenIssueDate = new DateTime(2023, 02, 10);
            DateTime refreshTokenExpiryDate = new DateTime(2023, 02, 16);
            var query = new NewSessionQuery(accessToken, refreshToken);
            NewSessionRequest newSessionRequest = new(accessToken, refreshToken);
            var handler = new NewSessionQueryHandler(_sessionProcessingService);
            UserRefreshToken userRefreshToken = UserRefreshToken.Create(refreshToken,refreshTokenIssueDate,refreshTokenExpiryDate);
            _jwtTokenProcessingServiceMock.Setup(x => x.Validate(It.IsAny<string>())).Returns(_claimsPrincipal);
            _accessTokenProviderMock.Setup(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(It.IsAny<string>());
            _userAccessRepositoryMock.Setup(x => x.GetUserRefreshToken(It.IsAny<string>())).Returns(userRefreshToken);

            //act
            Result<string> result = await handler.Handle(query, default);
            //assert
            _jwtTokenProcessingServiceMock.Verify(x => x.Validate(It.IsAny<string>()), Times.Once());
            _accessTokenProviderMock.Verify(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(),
                It.IsAny<string>()), Times.Never());
            _userAccessRepositoryMock.Verify(x => x.GetUserRefreshToken(It.IsAny<string>()), Times.Once());
            Assert.Equal(result.CustomProblemDetails, NewSessionLogicalValidationErrors.TokenExpiredError);
        }
        [Fact]
        public async Task ShouldReturnInvalidTokendErrorWhenStoredRefreshTokenIdIsNotSameAsAccessTokenUserId()
        {

            //arrange
            var claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.Name, "username"),
                    new Claim(ClaimTypes.NameIdentifier, "wrongUserId"),
                    new Claim("name", "John Doe"),
                };
            var identity = new ClaimsIdentity(claims, "TestAuthType");
            var claimsPrincipal = new ClaimsPrincipal(identity);
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiI0OTU4MDU1My02MGY0LTRhNTgtODdkYS1kYTg3NDY0M2NkZjUiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0MUBnbWFpbC5jb20iLCJqdGkiOiIxZTkwZTNhYS1jZWY3LTRmNDYtOGE2Mi1lYjdlNWFjOWMyYjEiLCJleHAiOjE2ODI2NzM0MjEsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MDAxIn0.rPBnHb_BROshOykqeThL5a_4bpTke8zYIMN6rAqst8A";
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0MUBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQ5NTgwNTUzLTYwZjQtNGE1OC04N2RhLWRhODc0NjQzY2RmNSIsImp0aSI6IjdmMjViOWQ2LTk5YzctNGRlMS04NzVhLTRlYmIzZGM2OWRiZiIsImV4cCI6MTY4MjY3NDMyMSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEifQ.blko_hnNJ5q99MveBtmjl2YaOhtEvV5t2DLVf02vwME";
            DateTime refreshTokenIssueDate = new DateTime(2023, 04, 10);
            DateTime refreshTokenExpiryDate = new DateTime(2023, 08, 16);
            var query = new NewSessionQuery(accessToken, refreshToken);
            NewSessionRequest newSessionRequest = new(accessToken, refreshToken);
            var handler = new NewSessionQueryHandler(_sessionProcessingService);
            UserRefreshToken userRefreshToken = UserRefreshToken.Create(refreshToken, refreshTokenIssueDate, refreshTokenExpiryDate);
            _jwtTokenProcessingServiceMock.Setup(x => x.Validate(It.IsAny<string>())).Returns(claimsPrincipal);
            _accessTokenProviderMock.Setup(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns(It.IsAny<string>());
            _userAccessRepositoryMock.Setup(x => x.GetUserRefreshToken(It.IsAny<string>())).Returns(userRefreshToken);

            //act
            Result<string> result = await handler.Handle(query, default);
            //assert
            _jwtTokenProcessingServiceMock.Verify(x => x.Validate(It.IsAny<string>()), Times.Once());
            _accessTokenProviderMock.Verify(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(),
                It.IsAny<string>()), Times.Never());
            _userAccessRepositoryMock.Verify(x => x.GetUserRefreshToken(It.IsAny<string>()), Times.Once());
            Assert.Equal(result.CustomProblemDetails, NewSessionLogicalValidationErrors.InvalidTokenError);
        }
        [Fact]
        public async Task ShouldReturnSuccessResultWhenNewSessionRequestIsSuccessful()
        {

            //arrange
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoic3RyaW5nIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiI0OTU4MDU1My02MGY0LTRhNTgtODdkYS1kYTg3NDY0M2NkZjUiLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0MUBnbWFpbC5jb20iLCJqdGkiOiIxZTkwZTNhYS1jZWY3LTRmNDYtOGE2Mi1lYjdlNWFjOWMyYjEiLCJleHAiOjE2ODI2NzM0MjEsImlzcyI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo1MDAxIn0.rPBnHb_BROshOykqeThL5a_4bpTke8zYIMN6rAqst8A";
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJ0ZXN0MUBnbWFpbC5jb20iLCJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1laWRlbnRpZmllciI6IjQ5NTgwNTUzLTYwZjQtNGE1OC04N2RhLWRhODc0NjQzY2RmNSIsImp0aSI6IjdmMjViOWQ2LTk5YzctNGRlMS04NzVhLTRlYmIzZGM2OWRiZiIsImV4cCI6MTY4MjY3NDMyMSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NTAwMSIsImF1ZCI6Imh0dHBzOi8vbG9jYWxob3N0OjUwMDEifQ.blko_hnNJ5q99MveBtmjl2YaOhtEvV5t2DLVf02vwME";
            DateTime refreshTokenIssueDate = new DateTime(2023, 04, 10);
            DateTime refreshTokenExpiryDate = new DateTime(2023, 08, 16);
            var query = new NewSessionQuery(accessToken, refreshToken);
            NewSessionRequest newSessionRequest = new(accessToken, refreshToken);
            var handler = new NewSessionQueryHandler(_sessionProcessingService);
            UserRefreshToken userRefreshToken = UserRefreshToken.Create(refreshToken, refreshTokenIssueDate, refreshTokenExpiryDate);
            _jwtTokenProcessingServiceMock.Setup(x => x.Validate(It.IsAny<string>())).Returns(_claimsPrincipal);
            _accessTokenProviderMock.Setup(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(),
                It.IsAny<string>())).Returns("newAccessToken");
            _userAccessRepositoryMock.Setup(x => x.GetUserRefreshToken(It.IsAny<string>())).Returns(userRefreshToken);

            //act
            Result<string> result = await handler.Handle(query, default);
            //assert
            _jwtTokenProcessingServiceMock.Verify(x => x.Validate(It.IsAny<string>()), Times.Once());
            _accessTokenProviderMock.Verify(x => x.GenerateAccessToken(It.IsAny<Guid>(), It.IsAny<string>(),
                It.IsAny<string>()), Times.Once());
            _userAccessRepositoryMock.Verify(x => x.GetUserRefreshToken(It.IsAny<string>()), Times.Once());
            Assert.Equal(ResultType.Ok,result.ResultType);
            Assert.Equal("newAccessToken", result.Data);

        }
    } */

}
