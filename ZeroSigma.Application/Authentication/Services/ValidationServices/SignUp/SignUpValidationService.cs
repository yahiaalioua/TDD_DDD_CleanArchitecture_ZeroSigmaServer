using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ZeroSigma.Application.Authentication.Services.Encryption;
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
        private readonly IEncryptionService _encryptionService;
        private readonly IUserRepository _userRepository;

        public SignUpValidationService(
            IUserRepository userRepository,
            IEncryptionService encryptionService
            )
        {
            _userRepository = userRepository;
            _encryptionService = encryptionService;
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
        public Result<SignUpResponse> ValidateUser(User? user, SignUpResponse signUpResponse)
        {
            if(!IsValidEmail(user.Email))
            {
                return new InvalidResult<SignUpResponse>(SignUpStructuralValidationErrors.InvalidEmailAddressError);
            }
            if (_userRepository.GetByEmail(user.Email) != null)
            {
                return new InvalidResult<SignUpResponse>(SignUpLogicalValidationErrors.DuplicateEmailError);
            }
            if (user.Password.Length < 9)
            {
                return new InvalidResult<SignUpResponse>(SignUpStructuralValidationErrors.InvalidPasswordLengthError);
            }
            if (user.Password.Length > 9 && user.Password.Length>70)
            {
                return new InvalidResult<SignUpResponse>(SignUpStructuralValidationErrors.InvalidPasswordLengthError);
            }
            if (!(Regex.IsMatch(user.Password, "[a-z]") && Regex.IsMatch(user.Password, "[A-Z]") && Regex.IsMatch(user.Password, "[0-9]")))
            {
                return new InvalidResult<SignUpResponse>(SignUpStructuralValidationErrors.InvalidPasswordError);
            }
            if (!Regex.IsMatch(user.Password, "[`,~,!,@,#,$,%,^,&,*,(,),_,-,+,=,{,[,},},|,\\,:,;,\",',<,,,>,.,?,/]"))
            {
                return new InvalidResult<SignUpResponse>(SignUpStructuralValidationErrors.MissingSpecialCharacterError);
            }
            if(user != null)
            {
                user.Password=_encryptionService.EncryptPassword(user.Password);
                _userRepository.Add(user);
            }
            return new SuccessResult<SignUpResponse>(signUpResponse);

        }
    }
}
