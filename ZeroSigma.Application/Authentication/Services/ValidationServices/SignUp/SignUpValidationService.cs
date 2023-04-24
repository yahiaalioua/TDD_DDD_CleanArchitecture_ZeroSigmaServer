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
        private bool IsValidEmail(string email)
        {
            string emailTrimed = email.Trim();

            if (!string.IsNullOrEmpty(emailTrimed))
            {
                bool hasWhitespace = emailTrimed.Contains(" ");

                int indexOfAtSign = emailTrimed.LastIndexOf('@');

                if (indexOfAtSign > 0 && !hasWhitespace)
                {
                    string afterAtSign = emailTrimed.Substring(indexOfAtSign + 1);

                    int indexOfDotAfterAtSign = afterAtSign.LastIndexOf('.');

                    if (indexOfDotAfterAtSign > 0 && afterAtSign.Substring(indexOfDotAfterAtSign).Length > 1)
                        return true;
                }
            }

            return false;
        }
        public Result<SignUpResponse> ValidateUser(RegisterRequest request)
        {
            var fullNameResult = FullName.Create(request.FullName);
            if (fullNameResult.ResultType == ResultType.Invalid)
            {
                return new InvalidResult<SignUpResponse>(fullNameResult.CustomProblemDetails);
            }
            if (!IsValidEmail(request.Email))
            {
                return new InvalidResult<SignUpResponse>(SignUpStructuralValidationErrors.InvalidEmailAddressError);
            }
            if (_userRepository.GetByEmail(request.Email) is not null)
            {
                return new InvalidResult<SignUpResponse>(SignUpLogicalValidationErrors.DuplicateEmailError);
            }
            if (request.Password.Length < 9)
            {
                return new InvalidResult<SignUpResponse>(SignUpStructuralValidationErrors.InvalidPasswordLengthError);
            }
            if (request.Password.Length > 9 && request.Password.Length>70)
            {
                return new InvalidResult<SignUpResponse>(SignUpStructuralValidationErrors.InvalidPasswordLengthError);
            }
            if (!(Regex.IsMatch(request.Password, "[a-z]") && Regex.IsMatch(request.Password, "[A-Z]") && Regex.IsMatch(request.Password, "[0-9]")))
            {
                return new InvalidResult<SignUpResponse>(SignUpStructuralValidationErrors.InvalidPasswordError);
            }
            if (!Regex.IsMatch(request.Password, "[`,~,!,@,#,$,%,^,&,*,(,),_,-,+,=,{,[,},},|,\\,:,;,\",',<,,,>,.,?,/]"))
            {
                return new InvalidResult<SignUpResponse>(SignUpStructuralValidationErrors.MissingSpecialCharacterError);
            }
            var validatedUser = _userProcessingService.CreateUser(fullNameResult.Data.Value, request.Email, request.Password, "", "");
            if (validatedUser is not null)
            {   
                validatedUser.Password=_encryptionService.EncryptPassword(request.Password);
                _userRepository.Add(validatedUser);
            }

            SignUpResponse response = new()
            {
                UserId = validatedUser.Id.Value,
                FullName = validatedUser.FullName.Value,
                Email = validatedUser.Email,
                Message = "You successfully registered"
            };
            return new SuccessResult<SignUpResponse>(response);

        }
    }
}
