using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Models;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Domain.ValueObjects.UserAccessBlackList;
using ZeroSigma.Domain.ValueObjects.UserRefreshToken;

namespace ZeroSigma.Domain.Entities
{
    public class UserAccessBlackList:Entity<BlackListId>
    {
        public UserAccessBlackList(
            BlackListId id, RefreshTokenID refreshTokenID,
            List<string> revokedRefreshTokens
            ) : base(id)
        {
            RefreshTokenID = refreshTokenID;
            RevokedRefreshTokens = revokedRefreshTokens;
        }

        public RefreshTokenID RefreshTokenID { get;  set; }
        public List<string> RevokedRefreshTokens { get;  set; }

        public static UserAccessBlackList Create(RefreshTokenID refreshTokenID)
        {
            return new UserAccessBlackList(BlackListId.CreateUnique(),refreshTokenID,new List<string>());
        }
    }
}
