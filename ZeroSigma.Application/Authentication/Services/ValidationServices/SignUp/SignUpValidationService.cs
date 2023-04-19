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

        public Result<SignUpResponse> ValidateUser(User? user, string fullName, string email, string password,SignUpResponse signUpResponse)
        {
            if (user != null)
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
      
            return new SuccessResult<SignUpResponse>(signUpResponse);

        }
    }
}
