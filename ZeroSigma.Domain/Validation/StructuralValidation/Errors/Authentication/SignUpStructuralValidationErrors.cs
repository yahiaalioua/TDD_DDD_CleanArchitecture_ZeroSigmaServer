using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Common.Errors;

namespace ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication
{
    public static class SignUpStructuralValidationErrors
    {
        //strictural SignUp Validation error codes should start with 100 example 101,102,103....
        public static readonly CustomProblemDetails InvalidPasswordLengthError = new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Password Length",
            Type = "Invalid Error",
            Detail = "Password length should be at least 8 characters and no more than 70 characters",
            Code = "100"
        };
        public static readonly CustomProblemDetails InvalidPasswordError = new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Password",
            Type = "Invalid Error",
            Detail = "Your password should be atleast 8 characters contain at least one number one upper and lowercase letter and one special character",
            Code = "101"
        };
        public static readonly CustomProblemDetails MissingSpecialCharacterError = new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Password",
            Type = "Invalid Error",
            Detail = "Your password should contain at least one special character",
            Code = "102"
        };

        public static CustomProblemDetails InvalidEmailAddressError = new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Email",
            Type = "Invalid Error",
            Detail = "The given email is invalid please use a valid email like example@mail.com",
            Code = "103"
        };
    }
}
