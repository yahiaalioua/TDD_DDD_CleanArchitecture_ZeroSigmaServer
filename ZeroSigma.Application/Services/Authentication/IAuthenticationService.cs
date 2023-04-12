using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.DTO.Authentication;

namespace ZeroSigma.Application.Services.Authentication
{
    public interface IAuthenticationService
    {
       SignUpResponse Register(RegisterRequest request);
       AuthenticationResponse Login(LoginRequest request);  
        
    }
}
