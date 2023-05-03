using MediatR;
using ZeroSigma.Application.Authentication.Services.ValidationServices.SignUp;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Common.Results;

namespace ZeroSigma.Application.Authentication.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<SignUpResponse>>
    {
        private readonly ISignUpValidationService _signUpValidationService;


        public RegisterCommandHandler(
            ISignUpValidationService signUpValidationService
            )
        {
            _signUpValidationService = signUpValidationService;
        }

        public async Task<Result<SignUpResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
          
            RegisterRequest registerRequest= new() { FullName = request.FullName,Email=request.Email,Password=request.Password };
            
            return await _signUpValidationService.ValidateUser(registerRequest);
        }
    }
}
