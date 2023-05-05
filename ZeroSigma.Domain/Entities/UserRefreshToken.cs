using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Models;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Domain.ValueObjects.UserRefreshToken;

namespace ZeroSigma.Domain.Entities
{
    public sealed class UserRefreshToken:Entity<RefreshTokenID>
    {
        public UserRefreshToken(
            RefreshTokenID id,
            string refreshToken, DateTime issuedDate,
            DateTime expiryDate, string isExpired
            ) : base(id)
        {
            RefreshToken = refreshToken;
            IssuedDate = issuedDate;
            ExpiryDate = expiryDate;
            IsExpired = isExpired;
        }
        public string RefreshToken { get;  set; } = null!;
        public DateTime IssuedDate { get;  set; }
        public DateTime ExpiryDate { get;  set;}
        public string IsExpired { get;  set; } = null!;

        public static UserRefreshToken Create(
            string refreshToken, DateTime issuedDate,
            DateTime expiryDate, string isExpired=null!
            )
        {
            if (expiryDate > DateTime.UtcNow)
            {
                isExpired = "yes";
            }
            isExpired = "no";
            return new UserRefreshToken(RefreshTokenID.CreateUnique(), refreshToken, issuedDate, expiryDate, isExpired);
        }
        public static UserRefreshToken CreateWithSameId(
            RefreshTokenID refreshTokenID,
            string refreshToken, DateTime issuedDate,
            DateTime expiryDate, string isExpired = null!
            )
        {
            if (expiryDate > DateTime.UtcNow)
            {
                isExpired = "yes";
            }
            isExpired = "no";
            return new UserRefreshToken(refreshTokenID, refreshToken, issuedDate, expiryDate, isExpired);
        }
    }
}
