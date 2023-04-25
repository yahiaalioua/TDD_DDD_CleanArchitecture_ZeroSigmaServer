using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Models;
using ZeroSigma.Domain.UserAccessAggregate.ValueObjects;

namespace ZeroSigma.Domain.Entities
{
    public sealed class UserAccessToken:Entity<AccessTokenID>
    {
        public UserAccessToken(
            AccessTokenID id, UserAccessID userAccessID,
            string accessToken, DateTime issuedDate,
            DateTime expiryDate, string isExpired
            ) : base(id)
        {
            UserAccessID = userAccessID;
            AccessToken = accessToken;
            IssuedDate = issuedDate;
            ExpiryDate = expiryDate;
            IsExpired = isExpired;
        }

        public UserAccessID UserAccessID { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
        public DateTime IssuedDate { get; set; }
        public DateTime ExpiryDate { get; set;}
        public string IsExpired { get; set; } = null!;

        public static UserAccessToken Create(
            UserAccessID userAccessID,
            string accessToken, DateTime issuedDate,
            DateTime expiryDate, string isExpired)
        {
            return new UserAccessToken(AccessTokenID.CreateUnique(), userAccessID, accessToken, issuedDate, expiryDate, isExpired);
        }
    }
}
