using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Authentication.Services.Encryption;
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
        private readonly IEncryptionService _encryptionService;

        public LoginValidationService(IEncryptionService encryptionService)
        {
            _encryptionService = encryptionService;
        }

        public Result<AuthenticationResponse> ValidateUser(User? user, string email, string password, string accessToken,string refreshToken)
        {
            if(user== null)
            {
                return new NotFoundResults<AuthenticationResponse>(LoginLogicalValidationErrors.NonExistentEmailError);
            }
            if(!_encryptionService.VerifyPassword(password,user.Password))
            {
                return new InvalidResult<AuthenticationResponse>(LoginLogicalValidationErrors.InvalidPasswordError);
            }
            AuthenticationResponse authenticationResponse = new()
            {
                Id = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                Message = "User Authenticated",
                AccessToken = accessToken,
            };
            return new SuccessResult<AuthenticationResponse>(authenticationResponse);
            
        }
    }
}
