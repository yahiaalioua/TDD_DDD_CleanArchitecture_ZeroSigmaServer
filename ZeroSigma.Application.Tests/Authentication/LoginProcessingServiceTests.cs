using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Authentication.Services.ProcessingServices.AuthenticationProcessingServices;
using ZeroSigma.Application.Authentication.TestData;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.Common.Interfaces;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Domain.ValueObjects.UserAccessToken;
using ZeroSigma.Infrastructure.Persistance.Repositories.IdentityAccess;

namespace ZeroSigma.Application.Authentication
{
    public class LoginProcessingServiceTests
    {
        private readonly AuthTestData _testData;
        private readonly ILoginProcessingService _loginProcessingService;
        private readonly Mock<IIdentityAccessRepository> _identityAccessRepositoryMock;
        private readonly Mock<IUnitOfWork> _IUnitOfWorkMock;

        public LoginProcessingServiceTests()
        {
            _testData = new AuthTestData();
            _identityAccessRepositoryMock = new Mock<IIdentityAccessRepository>();
            _IUnitOfWorkMock = new Mock<IUnitOfWork>();
            _loginProcessingService = new LoginProcessingService(
                _identityAccessRepositoryMock.Object,
                _IUnitOfWorkMock.Object
                );
        }

        [Fact]
        public void ShouldCallAddUserAccessTokenAsyncWhenUserHasNoIdentityAccessInDatabase()
        {
            //arrange
            var user = _testData._user;
            UserAccess identityAccess = null;
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            _identityAccessRepositoryMock.Setup(r => r.GetUserAccessByUserId(user.Id)).ReturnsAsync(identityAccess);
            _identityAccessRepositoryMock.Setup(x => x.AddUserAccessTokenAsync(It.IsAny<UserAccessToken>()));
            _IUnitOfWorkMock.Setup(x => x.SaveChangesAsync());
            //act            
            _loginProcessingService.ProcessAuthentication(user, accessToken, refreshToken);
            //assert
            _identityAccessRepositoryMock.Verify(x => x.GetUserAccessByUserId(user.Id),Times.Once);
            _identityAccessRepositoryMock.Verify(x => x.AddUserAccessTokenAsync(It.IsAny<UserAccessToken>()),Times.Once);
            _IUnitOfWorkMock.Verify(x=>x.SaveChangesAsync(),Times.Once);
        }
        [Fact]
        public void ShouldCallAddUserRefreshTokenAsyncWhenUserHasNoIdentityAccessInDatabase()
        {
            //arrange
            var user = _testData._user;
            UserAccess? identityAccess = null;
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            _identityAccessRepositoryMock.Setup(r => r.GetUserAccessByUserId(user.Id)).ReturnsAsync(identityAccess);
            _identityAccessRepositoryMock.Setup(x => x.AddUserRefreshTokenAsync(It.IsAny<UserRefreshToken>()));
            _IUnitOfWorkMock.Setup(x => x.SaveChangesAsync());
            //act            
            _loginProcessingService.ProcessAuthentication(user, accessToken, refreshToken);
            //assert
            _identityAccessRepositoryMock.Verify(x => x.GetUserAccessByUserId(user.Id), Times.Once);
            _identityAccessRepositoryMock.Verify(x => x.AddUserRefreshTokenAsync(It.IsAny<UserRefreshToken>()), Times.Once);
            _IUnitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public void ShouldCallAddUserAccessAsyncWhenUserHasNoIdentityAccessInDatabase()
        {
            //arrange
            var user = _testData._user;
            UserAccess? identityAccess = null;
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            _identityAccessRepositoryMock.Setup(r => r.GetUserAccessByUserId(user.Id)).ReturnsAsync(identityAccess);
            _identityAccessRepositoryMock.Setup(x => x.AddUserAccessAsync(It.IsAny<UserAccess>()));
            _IUnitOfWorkMock.Setup(x => x.SaveChangesAsync());
            //act            
            _loginProcessingService.ProcessAuthentication(user, accessToken, refreshToken);
            //assert
            _identityAccessRepositoryMock.Verify(x => x.GetUserAccessByUserId(user.Id), Times.Once);
            _identityAccessRepositoryMock.Verify(x => x.AddUserAccessAsync(It.IsAny<UserAccess>()), Times.Once);
            _IUnitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }
        [Fact]
        public void ShouldCallAddUserAccessBlacklistAsyncWhenUserHasNoIdentityAccessInDatabase()
        {
            //arrange
            var user = _testData._user;
            UserAccess? identityAccess = null;
            var accessToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            var refreshToken = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c";
            _identityAccessRepositoryMock.Setup(r => r.GetUserAccessByUserId(user.Id)).ReturnsAsync(identityAccess);
            _identityAccessRepositoryMock.Setup(x => x.AddUserAccessBlacklistAsync(It.IsAny<UserAccessBlackList>()));
            _IUnitOfWorkMock.Setup(x => x.SaveChangesAsync());
            //act            
            _loginProcessingService.ProcessAuthentication(user, accessToken, refreshToken);
            //assert
            _identityAccessRepositoryMock.Verify(x => x.GetUserAccessByUserId(user.Id), Times.Once);
            _identityAccessRepositoryMock.Verify(x => x.AddUserAccessBlacklistAsync(It.IsAny<UserAccessBlackList>()), Times.Once);
            _IUnitOfWorkMock.Verify(x => x.SaveChangesAsync(), Times.Once);
        }

    }
}
