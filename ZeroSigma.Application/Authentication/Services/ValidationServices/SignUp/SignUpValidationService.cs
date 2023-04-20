using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Errors;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;

namespace ZeroSigma.Application.Authentication.Services.ValidationServices.SignUp
{
    public class SignUpValidationService : ISignUpValidationService
    {
        private readonly IUserRepository _userRepository;

        public SignUpValidationService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
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
        public Result<SignUpResponse> ValidateUser(User? user, string fullName, string email, string password,SignUpResponse signUpResponse)
        {
            if(!IsValidEmail(email))
            {
                return new InvalidResult<SignUpResponse>(SignUpStructuralValidationErrors.InvalidEmailAddressError);
            }
            if (_userRepository.GetByEmail(email) != null)
            {
                return new InvalidResult<SignUpResponse>(SignUpLogicalValidationErrors.DuplicateEmailError);
            }
            if (password.Length < 9)
            {
                return new InvalidResult<SignUpResponse>(SignUpStructuralValidationErrors.InvalidPasswordLengthError);
            }
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
            {
                return new InvalidResult<SignUpResponse>(SignUpStructuralValidationErrors.InvalidPasswordError);
            }
            if (!Regex.IsMatch(password, "[`,~,!,@,#,$,%,^,&,*,(,),_,-,+,=,{,[,},},|,\\,:,;,\",',<,,,>,.,?,/]"))
            {
                return new InvalidResult<SignUpResponse>(SignUpStructuralValidationErrors.MissingSpecialCharacterError);
            }
            if(user != null)
            {
                _userRepository.Add(user);
            }
            return new SuccessResult<SignUpResponse>(signUpResponse);

        }
    }
}
