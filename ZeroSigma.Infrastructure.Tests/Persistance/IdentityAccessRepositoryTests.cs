using Microsoft.EntityFrameworkCore;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Infrastructure.Persistance.Repositories.IdentityAccess;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class IdentityAccessRepositoryTests:TestBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserAccess _userAccess;
        private readonly TestData _testData;
        private readonly UnitOfWork _unitOfWork;

        public IdentityAccessRepositoryTests()
        {
            _context = DbContext;
            _testData= new TestData();
            _userAccess = UserAccess.Create(_testData._user.Id,_testData._userAccessToken.Id,_testData._userRefreshToken.Id);
            _unitOfWork = new UnitOfWork(_context);
        }

        [Fact] 
        public async void AddUserAccessAsyncShouldAddUserAccessToDatabase()
        {
            //arrange
            
            var repository=new IdentityAccessRepository(_context);
            //act
            await _context.Users.AddAsync(_testData._user);
            await _context.UsersAccessToken.AddAsync(_testData._userAccessToken);
            await _context.UsersRefreshToken.AddAsync(_testData._userRefreshToken);
            await _unitOfWork.SaveChangesAsync();
            await repository.AddUserAccessAsync(_userAccess);
            await _unitOfWork.SaveChangesAsync();
            var data=await _context.UsersAccess.ToListAsync();
            //assert
            Assert.Contains(_userAccess,data);
            Assert.Single(data);            
        }
        [Fact]
        public async void GetUserAccessByUserIdShouldReturnUserAccessWhenUserAccesIsPresentInDatabase()
        {
            //arrange

            var repository = new IdentityAccessRepository(_context);
            //act
            await _context.Users.AddAsync(_testData._user);
            await _context.UsersAccessToken.AddAsync(_testData._userAccessToken);
            await _context.UsersRefreshToken.AddAsync(_testData._userRefreshToken);
            await _context.UsersAccess.AddAsync(_userAccess);
            await _unitOfWork.SaveChangesAsync();
            var data=await repository.GetUserAccessByUserId(_testData._user.Id);
            //assert
            Assert.Equal(_userAccess, data);

        }
        [Fact]
        public async void AddUserAccessTokenAsyncShouldAddUserAccessTokenToDatabase()
        {
            //arrange

            var repository = new IdentityAccessRepository(_context);
            //act
            await repository.AddUserAccessTokenAsync(_testData._userAccessToken);
            await _unitOfWork.SaveChangesAsync();
            var data = await _context.UsersAccessToken.ToListAsync();
            //assert
            Assert.Contains(_testData._userAccessToken, data);
        }

        [Fact]
        public async void GetUserAccessTokenByIdAsyncShouldReturnUserAccessToken()
        {
            //arrange

            var repository = new IdentityAccessRepository(_context);
            //act
            await repository.AddUserAccessTokenAsync(_testData._userAccessToken);
            await _unitOfWork.SaveChangesAsync();
            var data = await repository.GetUserAccessTokenByIdAsync(_testData._userAccessToken.Id);            
            //assert
            Assert.Equal(_testData._userAccessToken, data);
        }
        [Fact]
        public async void UpdateUserAccessTokenShouldUpdateUserAccessTokenInDatabase()
        {
            //arrange
            var newAccessToken = "newAccessToken";
            var newAccessTokenIssuedDate = new DateTime(2023, 06, 09);
            var newAccessTokenExpiryDate = new DateTime(2023, 06, 10);
            var repository = new IdentityAccessRepository(_context);
            var newUserAccessToken = UserAccessToken.Create
                (
                newAccessToken,
                newAccessTokenIssuedDate,newAccessTokenExpiryDate
                ); 
            //act
            await repository.AddUserAccessTokenAsync(_testData._userAccessToken);
            await _unitOfWork.SaveChangesAsync();
            await repository.UpdateUserAccessToken(_testData._userAccessToken.Id, newUserAccessToken);
            await _unitOfWork.SaveChangesAsync();
            var data = await repository.GetUserAccessTokenByIdAsync(_testData._userAccessToken.Id);
            //assert
            Assert.Equal(newUserAccessToken.AccessToken, data?.AccessToken);
            Assert.Equal(newUserAccessToken.IssuedDate, data?.IssuedDate);
            Assert.Equal(newUserAccessToken.ExpiryDate, data?.ExpiryDate);
            Assert.Equal(newUserAccessToken.IsExpired, data?.IsExpired);
            Assert.NotEqual(newUserAccessToken.Id,data?.Id);
        }

        [Fact]
        public async void AddUserRefreshTokenAsyncShouldAddUserRefreshTokenToDatabase()
        {
            //arrange

            var repository = new IdentityAccessRepository(_context);
            //act
            await repository.AddUserRefreshTokenAsync(_testData._userRefreshToken);
            await _unitOfWork.SaveChangesAsync();
            var data = await _context.UsersRefreshToken.ToListAsync();            
            //assert
            Assert.Contains(_testData._userRefreshToken, data);
        }
        [Fact]
        public async void GetUserRefreshTokenByIdAsyncShouldReturnUserRefreshToken()
        {
            //arrange

            var repository = new IdentityAccessRepository(_context);
            //act
            await repository.AddUserRefreshTokenAsync(_testData._userRefreshToken);
            await _unitOfWork.SaveChangesAsync();
            var data = await repository.GetUserRefreshTokenByIdAsync(_testData._userRefreshToken.Id);
            //assert
            Assert.Equal(_testData._userRefreshToken, data);
        }
        [Fact]
        public async void UpdateUserRefreshTokenShouldUpdateUserRefreshTokenInDatabase()
        {
            //arrange
            var newRefreshToken= "newRefreshToken";
            var newRefreshTokenIssuedDate = new DateTime(2023, 06, 09);
            var newRefreshTokenExpiryDate = new DateTime(2023,06,10);
            var repository = new IdentityAccessRepository(_context);
            var newUserRefreshToken = UserRefreshToken.Create
                (
                newRefreshToken, newRefreshTokenIssuedDate,
                newRefreshTokenExpiryDate
                );
            //act
            await repository.AddUserRefreshTokenAsync(_testData._userRefreshToken);
            await _unitOfWork.SaveChangesAsync();
            await repository.UpdateUserRefreshToken(_testData._userRefreshToken.Id,newUserRefreshToken);
            await _unitOfWork.SaveChangesAsync();
            var data = await repository.GetUserRefreshTokenByIdAsync(_testData._userRefreshToken.Id);
            //assert
            Assert.Equal(newUserRefreshToken.RefreshToken, data?.RefreshToken);
            Assert.Equal(newUserRefreshToken.IssuedDate, data?.IssuedDate);
            Assert.Equal(newUserRefreshToken.ExpiryDate, data?.ExpiryDate);
            Assert.Equal(newUserRefreshToken.IsExpired, data?.IsExpired);
            Assert.NotEqual(newUserRefreshToken.Id, data?.Id);
        }

        [Fact]
        public async void AddUserAccessBlacklistAsyncShouldAddUserAccessBlacklistToDatabase()
        {
            //arrange

            var repository = new IdentityAccessRepository(_context);
            //act
            await repository.AddUserRefreshTokenAsync(_testData._userRefreshToken);
            await repository.AddUserAccessBlacklistAsync(_testData._accessBlackList);
            await _unitOfWork.SaveChangesAsync();
            var data = await _context.UsersAccessBlackLists.ToListAsync();
            //assert
            Assert.Contains(_testData._accessBlackList, data);
        }
        [Fact]
        public async void GetUserAccessBlacklistAsyncShouldReturnUserAccessBlacklist()
        {
            //arrange

            var repository = new IdentityAccessRepository(_context);
            //act
            await repository.AddUserRefreshTokenAsync(_testData._userRefreshToken);
            await repository.AddUserAccessBlacklistAsync(_testData._accessBlackList);
            await _unitOfWork.SaveChangesAsync();
            var data = await repository.GetUserAccessBlacklistAsync(_testData._userRefreshToken.Id);
            //assert
            Assert.Equal(_testData._accessBlackList, data);
        }
    }
}
