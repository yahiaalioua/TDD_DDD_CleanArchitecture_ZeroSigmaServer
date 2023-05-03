using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Common.Interfaces;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Infrastructure.Persistance.Repositories.Users;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class UserRepositoryTests : TestBase
    {
        private readonly ApplicationDbContext _context;
        private readonly TestData _testData;
        private readonly UnitOfWork _unitOfWork;
        public UserRepositoryTests()
        {
            _context = DbContext;
            _testData = new TestData();
            _unitOfWork=new UnitOfWork(_context);
        }

        [Fact]
        public async void ShouldAddUserToDatabaseViaUserRepositoryAddUserAyncMethod()
        {
            //arrange
            var repository = new UserRepository(_context);
            //act
            await repository.AddUserAsync(_testData._user);
            await _unitOfWork.SaveChangesAsync();
            var data=await _context.Users.ToListAsync();
            //assert
            Assert.Contains(_testData._user, data);
        }
        [Fact]
        public async void GetByEmailAsyncShouldReturnUserWhenUserExistsInDatabase()
        {
            //arrange
            var repository = new UserRepository(_context);
            //act
            await repository.AddUserAsync(_testData._user);
            await _unitOfWork.SaveChangesAsync();
            var data=await repository.GetByEmailAsync(_testData._user.Email);
            //assert
            Assert.Equal(_testData._user, data);
        }
        [Fact]
        public async void GetByIdAsyncShouldReturnUserWhenUserExistsInDatabase()
        {
            //arrange
            var repository = new UserRepository(_context);
            //act
            await repository.AddUserAsync(_testData._user);
            await _unitOfWork.SaveChangesAsync();
            var data = await repository.GetByIdAsync(_testData._user.Id);
            //assert
            Assert.Equal(_testData._user, data);
        }
    }
}
