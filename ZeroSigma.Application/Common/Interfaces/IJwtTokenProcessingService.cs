using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSigma.Application.Common.Interfaces
{
    public interface IJwtTokenProcessingService
    {
        public ClaimsPrincipal Validate(string refreshToken);
    }
}
