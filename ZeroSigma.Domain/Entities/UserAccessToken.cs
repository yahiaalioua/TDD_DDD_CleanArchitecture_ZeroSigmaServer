using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Models;
using ZeroSigma.Domain.ValueObjects.UserAccess;
using ZeroSigma.Domain.ValueObjects.UserAccessToken;

namespace ZeroSigma.Domain.Entities
{
    public sealed class UserAccessToken:Entity<AccessTokenID>
    {
        public UserAccessToken(
            AccessTokenID id,
            string accessToken, DateTime issuedDate,
            DateTime expiryDate, string isExpired
            ) : base(id)
        {
            AccessToken = accessToken;
            IssuedDate = issuedDate;
            ExpiryDate = expiryDate;
            IsExpired = isExpired;
        }
        public string AccessToken { get;  set; } = null!;
        public DateTime IssuedDate { get;  set; }
        public DateTime ExpiryDate { get;  set;}
        public string IsExpired { get;  set; } = null!;

        public static UserAccessToken Create(
            string accessToken, DateTime issuedDate,
            DateTime expiryDate, string isExpired=null!)
        {
            if (expiryDate < DateTime.UtcNow)
            {
                isExpired = "yes";
            }
            else
            {
                isExpired = "no";
            }
            return new UserAccessToken(AccessTokenID.CreateUnique(), accessToken, issuedDate, expiryDate,isExpired);
        }

        public static UserAccessToken CreateWithSameId(
            AccessTokenID id,
            string accessToken, DateTime issuedDate,
            DateTime expiryDate, string isExpired = null!)
        {
            if (expiryDate < DateTime.UtcNow)
            {
                isExpired = "yes";
            }
            else
            {
                isExpired = "no";
            }
            return new UserAccessToken(id, accessToken, issuedDate, expiryDate, isExpired);
        }
    }
}
