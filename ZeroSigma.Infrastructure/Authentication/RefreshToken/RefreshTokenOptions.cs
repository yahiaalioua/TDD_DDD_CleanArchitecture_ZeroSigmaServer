using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSigma.Infrastructure.Authentication.RefreshToken
{
    public class RefreshTokenOptions
    {
        public int RefreshTokenExpirationMinutes { get; init; }
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public string RefreshTokenSecretKey { get; init; }=null!;
    }
}
