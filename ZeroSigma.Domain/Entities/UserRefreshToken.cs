using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Models;
using ZeroSigma.Domain.UserAccessAggregate.ValueObjects;

namespace ZeroSigma.Domain.Entities
{
    public sealed class UserRefreshToken:Entity<RefreshTokenID>
    {
        public UserRefreshToken(
            RefreshTokenID id, UserAccessID userAccessID,
            string refreshToken, DateTime issuedDate,
            DateTime expiryDate, string isExpired
            ) : base(id)
        {
            UserAccessID = userAccessID;
            RefreshToken = refreshToken;
            IssuedDate = issuedDate;
            ExpiryDate = expiryDate;
            IsExpired = isExpired;
        }

        public UserAccessID UserAccessID { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime IssuedDate { get; set; }
        public DateTime ExpiryDate { get; set;}
        public string IsExpired { get; set; } = null!;

        public static UserRefreshToken Create(
            UserAccessID userAccessID,
            string refreshToken, DateTime issuedDate,
            DateTime expiryDate, string isExpired
            )
        {
            return new UserRefreshToken(RefreshTokenID.CreateUnique(), userAccessID, refreshToken, issuedDate, expiryDate, isExpired);
        }
    }
}
