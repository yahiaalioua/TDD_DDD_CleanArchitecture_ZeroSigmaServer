using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZeroSigma.Application.Authentication.Services.Encryption;
using ZeroSigma.Application.Authentication.Services.ProcessingServices;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Errors;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.User.ValueObjects;
using ZeroSigma.Domain.UserAggregate.ValueObjects;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;

namespace ZeroSigma.Application.Authentication.Services.ValidationServices.SignUp
{
    public class SignUpValidationService : ISignUpValidationService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IUserRepository _userRepository;
        private readonly IUserProcessingService _userProcessingService;

        public SignUpValidationService(
            IUserRepository userRepository,
            IEncryptionService encryptionService
,
            IUserProcessingService userProcessingService)
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
            _userProcessingService = userProcessingService;
        }

        
        public Result<SignUpResponse> ValidateUser(RegisterRequest request)
        {
            var fullNameResult = FullName.Create(request.FullName);
            var userEmailResult = UserEmail.Create(request.Email);
            var userPassword=UserPassword.Create(request.Password);
            if (fullNameResult.ResultType == ResultType.Invalid)
            {
                return new InvalidResult<SignUpResponse>(fullNameResult.CustomProblemDetails);
            }
            if (userEmailResult.ResultType == ResultType.Invalid)
            {
                return new InvalidResult<SignUpResponse>(userEmailResult.CustomProblemDetails);
            }
            if(userPassword.ResultType == ResultType.Invalid)
            {
                return new InvalidResult<SignUpResponse>(userPassword.CustomProblemDetails);
            }
            if (_userRepository.GetByEmail(userEmailResult.Data) is not null)
            {
                return new InvalidResult<SignUpResponse>(SignUpLogicalValidationErrors.DuplicateEmailError);
            }
            var validatedUser = _userProcessingService.CreateUser(fullNameResult.Data, userEmailResult.Data,userPassword.Data);
            if (validatedUser is not null)
            {   
                string encryptedPassword=_encryptionService.EncryptPassword(userPassword.Data.Value);
                validatedUser.Password = UserPassword.Create(encryptedPassword).Data;
                _userRepository.Add(validatedUser);
            }

            SignUpResponse response = new()
            {
                UserId = validatedUser.Id.Value,
                FullName = validatedUser.FullName.Value,
                Email = validatedUser.Email.Value,
                Message = "You successfully registered"
            };
            return new SuccessResult<SignUpResponse>(response);

        }
    }
}
