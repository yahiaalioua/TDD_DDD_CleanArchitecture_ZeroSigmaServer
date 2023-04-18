using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Errors;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.Validation.StructuralValidation.Errors.Authentication;

namespace ZeroSigma.Application.Authentication.Commands
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<SignUpResponse>>
    {
        private readonly IUserRepository _userRepository;
        

        public RegisterCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
           
        }
        
        public async Task<Result<SignUpResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            User user = new() { FullName=request.FullName, Email=request.Email,Password=request.Password };
            if (_userRepository.GetByEmail(request.Email) != null)
            {
                CustomProblemDetails DuplicateEmailError = SignUpStructuralValidationErrors.DuplicateEmailError;
                return new InvalidResult<SignUpResponse>(DuplicateEmailError);
            }
            _userRepository.Add(user);
            SignUpResponse response = new() { UserId = user.Id,FullName=user.FullName, Email = user.Email, Message = "You successfully registered"};
            return new SuccessResult<SignUpResponse>(response);
            
        }
    }
}
