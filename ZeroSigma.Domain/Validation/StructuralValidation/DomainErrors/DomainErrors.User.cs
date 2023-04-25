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
        
        public static readonly CustomProblemDetails InvalidEmailAddressError = new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Email",
            Type = "Invalid Error",
            Detail = "The given email is invalid please use a valid email like example@mail.com",
            Code = "303"
        };
        public static readonly CustomProblemDetails EmptyPasswordError = new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Password",
            Type = "Invalid Error",
            Detail = "Password cannot be empty",
            Code = "304"
        };
        public static readonly CustomProblemDetails InvalidPasswordLengthError = new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Password Length",
            Type = "Invalid Error",
            Detail = "Password length should be at least 8 characters and no more than 70 characters",
            Code = "305"
        };
        public static readonly CustomProblemDetails InvalidPasswordError= new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Password",
            Type = "Invalid Error",
            Detail = "Your password should be atleast 8 characters contain at least one number one upper and lowercase letter and one special character",
            Code = "306"
        };
        public static readonly CustomProblemDetails MissingSpecialCharacterError= new()
        {
            Status = HttpStatusCode.BadRequest,
            Title = "Invalid Password",
            Type = "Invalid Error",
            Detail = "Your password should contain at least one special character",
            Code = "102"
        };
    }
}
