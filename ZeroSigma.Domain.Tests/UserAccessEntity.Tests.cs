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
        private readonly UserAccessToken _expiredUserAccessToken;
        private readonly UserAccessToken _validUserAccessToken;
        public UserAccessEntity()
        {
            _expiredUserAccessToken = UserAccessToken.Create("accessToken",new DateTime(2023,02,20), new DateTime(2020, 02, 22));
            _validUserAccessToken = UserAccessToken.Create("accessToken", new DateTime(2023, 04, 20), new DateTime(2023, 06, 12));

        }

        [Fact]
        public void IsExpiredShouldReturnYesWhenAccessTokenIsExpired()
        {
            //arrange
            //act
            //assert
            Assert.Equal("yes",_expiredUserAccessToken.IsExpired);
        }
        [Fact]
        public void IsExpiredShouldReturnNoWhenAccessTokenIsNotExpired()
        {
            //arrange
            //act
            //assert
            Assert.Equal("no", _validUserAccessToken.IsExpired);
        }
    }
}
