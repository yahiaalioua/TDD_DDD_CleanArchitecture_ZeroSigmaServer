using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Errors;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;

namespace ZeroSigma.Application.Authentication.Services.ValidationServices.Login
{
    public class LoginValidationService:ILoginValidationService
    {

        public Result<AuthenticationResponse> ValidateUser(User? user, string email, string password, string accessToken,string refreshToken)
        {
            if(user== null)
            {
                return new NotFoundResults<AuthenticationResponse>(LoginLogicalValidationErrors.NonExistentEmailError);
            }
            if(password!=user.Password)
            {
                return new InvalidResult<AuthenticationResponse>(LoginLogicalValidationErrors.InvalidPasswordError);
            }
            AuthenticationResponse response = new()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                Message="User Authenticated"
            };
            return new SuccessResult<AuthenticationResponse>(response);
            
        }
    }
}
