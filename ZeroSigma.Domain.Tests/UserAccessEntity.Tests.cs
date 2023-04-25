using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.User.ValueObjects;
using ZeroSigma.Domain.UserAccessAggregate.ValueObjects;

namespace ZeroSigma.Domain.Tests
{
    public class UserAccessEntity
    {
        private readonly UserAccessToken _userAccessToken;
        public UserAccessEntity()
        {
            _userAccessToken = UserAccessToken.Create("accessToken",new DateTime(2023,02,20), new DateTime(2023, 02, 22));

        }

        [Fact]
        public void IsExpiredShouldReturnYesWhenAccessTokenIsExpired()
        {
            //arrange
            //act
            //assert
            Assert.Equal("yes",_userAccessToken.IsExpired);
        }
    }
}
