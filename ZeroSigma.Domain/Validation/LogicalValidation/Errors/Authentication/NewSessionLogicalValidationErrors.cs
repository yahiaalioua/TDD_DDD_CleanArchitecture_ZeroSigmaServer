using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Common.Errors;

namespace ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication
{
    public static class NewSessionLogicalValidationErrors
    {
        // New session logical validation errors codes should start with 700, 701, 702,703....
        public static readonly CustomProblemDetails InvalidTokenError = new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Token Error",
            Type = "Invalid Session Error",
            Detail = "The given token is invalid",
            Code = "700"
        };
        public static readonly CustomProblemDetails TokenNotFoundError = new()
        {
            Status = HttpStatusCode.NotFound,
            Title = "Token Not Found",
            Type = "Not Found Error",
            Detail = "The given refresh token was not found",
            Code = "701"
        };
        public static readonly CustomProblemDetails TokenExpiredError = new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Expired Token",
            Type = "Invalid Token Error",
            Detail = "The given refresh token is expired",
            Code = "702"
        };
        public static readonly CustomProblemDetails TokenReusedError = new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Token Reused",
            Type = "Compromised Session",
            Detail = "Warning the token might have been compromised",
            Code = "703"
        };
    }
}
