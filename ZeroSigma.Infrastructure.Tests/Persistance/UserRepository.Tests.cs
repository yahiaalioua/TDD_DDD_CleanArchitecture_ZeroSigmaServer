using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Entities;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class UserPersistance
    {
        private User _user1 { get; set; }
        private List<User> _userDb { get; set; }
        private UserRepository _userRepository { get; set; }
        public UserPersistance()
        {
            // arrange
            _user1 = new User()
            {
                Name = "yahia",
                Email = "test@mail.com",
                Password = "dummmypass",
                AccessToken = "dummyAccessToken",
                RefreshToken = "dummyRefreshToken"
            };
            _userDb = new();
            _userRepository = new UserRepository(_userDb);
        }

        [Fact]
        public void ShouldAddUserToUserFakeDb()
        {
            //arrange
            
            
            //act
            _userRepository.Add(_user1);
            var savedUser = _userDb.Single();
            //assert
            Assert.Equal(savedUser, _user1);
            Assert.Contains(_user1, _userDb);
        }

        [Fact]
        public void ShouldReturnUserIfEmailIsAvailabeInFakeDb ()
        {
            //arrange 

            //act
            _userRepository.Add(_user1);
            User? result= _userRepository.GetByEmail(_user1.Email);
            //assert
            Assert.Equal(_user1, result);
            Assert.Contains(_user1.Name, result?.Name);
        }
    }
}
