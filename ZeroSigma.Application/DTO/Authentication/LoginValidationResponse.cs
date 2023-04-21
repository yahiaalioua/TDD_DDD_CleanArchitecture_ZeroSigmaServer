using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSigma.Application.DTO.Authentication
{
    public record LoginValidationResponse
    {
        public AuthenticationResponse AuthenticationResponse { get; set; } = null!;
        public string AccessToken { get; set; } = null!;
    }
}
