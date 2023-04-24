using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Common.Errors;

namespace ZeroSigma.Domain.Validation.StructuralValidation.DomainErrors
{
    public static class DomainErrors
    {
        //User domain errors code shoudl start with 300, like 301, 302....
        public static readonly CustomProblemDetails EmptyFullNameError=new CustomProblemDetails()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Full Name",
            Type = "Invalid Error",
            Detail = "Full name cannot be empty",
            Code = "300"
        };
        public static readonly CustomProblemDetails InvalidFullNameLengthError= new CustomProblemDetails()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Full Name",
            Type = "Invalid Error",
            Detail = "Full name length should not be more than 50 characters",
            Code = "301"
        };
        public static readonly CustomProblemDetails EmptyEmailError = new CustomProblemDetails()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Email",
            Type = "Invalid Error",
            Detail = "Email caanot be empty",
            Code = "302"
        };
        public static CustomProblemDetails InvalidEmailAddressError = new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Email",
            Type = "Invalid Error",
            Detail = "The given email is invalid please use a valid email like example@mail.com",
            Code = "303"
        };
    }
}
