using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;

namespace ZeroSigma.Application.Authentication.Services.ValidationServices.Login
{
    public interface ILoginValidationService
    {
        Result<AuthenticationResponse> ValidateUser(
            User? user,
            string email, string password,
            string accessToken, string refreshToken
            );
    }
}
