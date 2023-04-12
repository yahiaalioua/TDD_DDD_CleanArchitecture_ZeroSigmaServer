using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSigma.Application.Common.Authentication
{
    public interface IJwtGenerator
    {
        string GenerateJwt(string secretKey, string issuer, string audience, double expires, List<Claim> claims = null!);
    }
}
