using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZeroSigma.Application.Authentication.Services.Encryption;
using ZeroSigma.Application.Authentication.Services.ProcessingServices.SignUpProcessingServices;
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
        private readonly ISignUpProcessingService _userProcessingService;
        private readonly IUserRepository _userRepository;

        public SignUpValidationService(
            ISignUpProcessingService userProcessingService,
            IUserRepository userRepository
            )
        {
            _userProcessingService = userProcessingService;
            _userRepository = userRepository;
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
            var validatedUser=_userProcessingService.ProcessSignUpRequest(fullNameResult.Data, userEmailResult.Data, userPassword.Data);
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
