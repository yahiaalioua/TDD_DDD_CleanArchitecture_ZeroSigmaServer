using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Authentication.Services.ValidationServices.Login;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Errors;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.User.ValueObjects;
using ZeroSigma.Domain.UserAggregate.ValueObjects;

namespace ZeroSigma.Application.Authentication.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<AuthenticationResponse>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ILoginValidationService _loginValidationService;

        public LoginQueryHandler(
            IUserRepository userRepository,
            ILoginValidationService loginValidationService
            )
        {
            _userRepository = userRepository;
            _loginValidationService = loginValidationService;
        }

        public async Task<Result<AuthenticationResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var email=UserEmail.Create(request.Email);
            if (email.ResultType == ResultType.Invalid)
            {
                return new InvalidResult<AuthenticationResponse>(email.CustomProblemDetails);
            }
            User? existingUser=_userRepository.GetByEmail(email.Data);
            
            return _loginValidationService.ValidateUser(existingUser,request.Password);          
            
        }
    }
}
