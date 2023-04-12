using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSigma.Infrastructure.Authentication.AccessToken
{
    public class AccessTokenOptions
    {
        public double AccessTokenExpirationMinutes { get; init; }
        public string Issuer { get; init; } = null!;
        public string Audience { get; init; } = null!;
        public string AccessTokenSecretKey { get; init; } = null!;
    }
}
