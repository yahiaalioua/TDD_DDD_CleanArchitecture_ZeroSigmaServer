using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Infrastructure.Persistance.Repositories.Users;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class UserRepositoryTests : TestBase
    {
        private readonly FullName _fullname;
        private readonly UserEmail _email;
        private readonly UserPassword _password;
        private readonly User _user;
        private readonly ApplicationDbContext _context;

        public UserRepositoryTests()
        {
            _fullname = FullName.Create("Mike").Data;
            _email = UserEmail.Create("test@mail.com").Data;
            _password = UserPassword.Create("TestPass3459$%.").Data;
            _user = User.Create(_fullname, _email, _password);
            _context = DbContext;
        }

        [Fact]
        public async void ShouldAddUserToDatabaseViaUserRepositoryAddUserAyncMethod()
        {
            //arrange
            var repository = new UserRepository(_context);
            //act
            await repository.AddUserAsync(_user);
            var data=await _context.Users.ToListAsync();
            //assert
            Assert.Contains(_user, data);
        }
        [Fact]
        public async void GetByEmailAsyncShouldReturnUserWhenUserExistsInDatabase()
        {
            //arrange
            var repository = new UserRepository(_context);
            //act
            await repository.AddUserAsync(_user);
            var data=await repository.GetByEmailAsync(_email);
            //assert
            Assert.Equal(_user, data);
        }
        [Fact]
        public async void GetByIdAsyncShouldReturnUserWhenUserExistsInDatabase()
        {
            //arrange
            var repository = new UserRepository(_context);
            //act
            await repository.AddUserAsync(_user);
            var data = await repository.GetByIdAsync(_user.Id);
            //assert
            Assert.Equal(_user, data);
        }
    }
}
