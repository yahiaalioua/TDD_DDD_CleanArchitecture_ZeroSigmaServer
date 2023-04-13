using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSigma.Application.Common.Authentication
{
    public interface IRefreshTokenProvider
    {
        string GenerateRefreshToken(Guid id, string email);
    }
}
