using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Common.Errors;

namespace ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication
{
    public static class LoginLogicalValidationErrors
    {
        public static readonly CustomProblemDetails NonExistentEmailError = new()
        {
            Status = HttpStatusCode.NotFound,
            Title = "Non Existent Email Error",
            Type = "NotFound",
            Detail = "The given email do not exist in our system",
            Code = "002"
        };
        public static readonly CustomProblemDetails InvalidPasswordError = new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Password",
            Type = "Invalid Error",
            Detail = "Wrong password, make sure to type the correct password",
            Code = "002"
        };
        public static CustomProblemDetails InvalidPasswordLengthError;
    }
}
