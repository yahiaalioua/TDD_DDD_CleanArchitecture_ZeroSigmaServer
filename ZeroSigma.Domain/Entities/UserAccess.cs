using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Models;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Domain.ValueObjects.UserAccess;
using ZeroSigma.Domain.ValueObjects.UserAccessToken;
using ZeroSigma.Domain.ValueObjects.UserRefreshToken;

namespace ZeroSigma.Domain.Entities
{
    public sealed class UserAccess : Entity<UserAccessID>
    {
        public UserAccess(UserAccessID id, UserID userID,
            AccessTokenID accessTokenID, RefreshTokenID refreshTokenID) : base(id)
        {
            UserID = userID;
            AccessTokenID = accessTokenID;
            RefreshTokenID = refreshTokenID;
        }
        public UserID UserID { get; set; }
        public AccessTokenID AccessTokenID { get;  set; }
        public RefreshTokenID RefreshTokenID { get;  set; }

        public static UserAccess Create(
            UserID userId, AccessTokenID accessTokenID,
            RefreshTokenID refreshTokenID
            )
        {
            return new UserAccess( UserAccessID.CreateUnique(), userId, accessTokenID, refreshTokenID );
        }

    }
}
