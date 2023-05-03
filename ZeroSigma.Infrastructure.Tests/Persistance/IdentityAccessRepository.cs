using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Domain.ValueObjects.UserAccessToken;
using ZeroSigma.Domain.ValueObjects.UserRefreshToken;
using ZeroSigma.Infrastructure.Persistance.Repositories.IdentityAccess;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class IdentityAccessRepositoryTests:TestBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserAccess _userAccess;
        private readonly TestData _testData;

        public IdentityAccessRepositoryTests()
        {
            _context = DbContext;
            _testData= new TestData();
            _userAccess = UserAccess.Create(_testData._user.Id,_testData._userAccessToken.Id,_testData._userRefreshToken.Id);
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
            await _context.SaveChangesAsync();
            await repository.AddUserAccessAsync(_userAccess);
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
            await _context.SaveChangesAsync();
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
            var data = await _context.UsersAccessToken.ToListAsync();
            await _context.SaveChangesAsync();
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
            var data = await repository.GetUserAccessTokenByIdAsync(_testData._userAccessToken.Id);            
            //assert
            Assert.Equal(_testData._userAccessToken, data);
        }

        [Fact]
        public async void AddUserRefreshTokenAsyncShouldAddUserRefreshTokenToDatabase()
        {
            //arrange

            var repository = new IdentityAccessRepository(_context);
            //act
            await repository.AddUserRefreshTokenAsync(_testData._userRefreshToken);
            var data = await _context.UsersRefreshToken.ToListAsync();
            await _context.SaveChangesAsync();
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
            var data = await repository.GetUserRefreshTokenByIdAsync(_testData._userRefreshToken.Id);
            //assert
            Assert.Equal(_testData._userRefreshToken, data);
        }
    }
}
