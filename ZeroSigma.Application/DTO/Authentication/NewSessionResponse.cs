using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSigma.Application.DTO.Authentication
{
    public record NewSessionResponse
    (
        string AccessToken,
        string RefreshToken
        );
}
