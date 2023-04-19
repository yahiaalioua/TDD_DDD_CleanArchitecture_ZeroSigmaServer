using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;

namespace ZeroSigma.Application.Authentication.Services.ValidationServices.SignUp
{
    public interface ISignUpValidationService
    {
        Result<SignUpResponse> ValidateUser(User? user, string fullName, string email, string password,SignUpResponse signUpResponse);
    }
}