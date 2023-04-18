using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Common.Errors;

namespace ZeroSigma.Domain.Validation.StructuralValidation.Errors.Authentication
{
    public static class SignUpStructuralValidationErrors
    {
        public static readonly CustomProblemDetails DuplicateEmailError = new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Duplicate email exception",
            Type = "Duplicate exception",
            Detail = "Email already exist in our system",
            Code = "001"
        };
    }
}
