﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Authentication.Services.Encryption;
using ZeroSigma.Application.Authentication.Services.ProcessingServices.AuthenticationProcessingServices;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Errors;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.User.ValueObjects;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;

namespace ZeroSigma.Application.Authentication.Services.ValidationServices.Login
{
    public class LoginValidationService:ILoginValidationService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly ILoginProcessingService _loginProcessingService;

        public LoginValidationService(
            IEncryptionService encryptionService,
            ILoginProcessingService loginProcessingService)
        {
            _encryptionService = encryptionService;
            _loginProcessingService = loginProcessingService;
        }

        public Result<AuthenticationResponse> ValidateUser(User? user, string password)
        {
            if(user is null)
            {
                return new NotFoundResults<AuthenticationResponse>(LoginLogicalValidationErrors.NonExistentEmailError);
            }
            if(!_encryptionService.VerifyPassword(password,user.Password.Value))
            {
                return new InvalidResult<AuthenticationResponse>(LoginLogicalValidationErrors.InvalidPasswordError);
            }
            var accessToken=_loginProcessingService.ProcessAuthentication(user);
            AuthenticationResponse authenticationResponse = new()
            {
                Id = user.Id.Value,
                FullName = user.FullName.Value,
                Email = user.Email.Value,
                Message = "User Authenticated",
                AccessToken = accessToken,
            };
            return new SuccessResult<AuthenticationResponse>(authenticationResponse);
            
        }
    }
}
