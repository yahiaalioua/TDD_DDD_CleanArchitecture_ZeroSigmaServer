using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Domain.ValueObjects.UserAccessToken;
using ZeroSigma.Domain.ValueObjects.UserRefreshToken;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class TestData
    {
        public readonly FullName _fullname;
        public readonly UserEmail _email;
        public readonly UserPassword _password;
        public readonly User _user;
        public readonly RefreshTokenID _refreshTokenID;
        public readonly AccessTokenID _accessTokenID;
        public readonly UserAccessToken _userAccessToken;
        public readonly UserRefreshToken _userRefreshToken;

        public TestData()
        {
            _fullname = FullName.Create("Mike").Data;
            _email = UserEmail.Create("test@mail.com").Data;
            _password = UserPassword.Create("TestPass3459$%.").Data;
            _user = User.Create(_fullname, _email, _password);
            _accessTokenID = AccessTokenID.CreateUnique();
            _refreshTokenID = RefreshTokenID.CreateUnique();
            _userAccessToken = UserAccessToken.Create("accessTokenFake", new DateTime(2023, 01, 10), new DateTime(2023, 01, 10));
            _userRefreshToken = UserRefreshToken.Create(_user.Id,"accessTokenFake", new DateTime(2023, 01, 10), new DateTime(2023, 01, 10));
        }
    }
}
