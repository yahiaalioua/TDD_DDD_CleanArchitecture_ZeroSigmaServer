using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Authentication.Services.ValidationServices.SignUp;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Errors;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;

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
            User createdUser = new() { FullName = request.FullName, Email = request.Email, Password = request.Password };
            
            SignUpResponse response = new() {
                UserId =createdUser.Id, FullName = createdUser.FullName,
                Email = createdUser.Email, Message = "You successfully registered" 
            };
            
            return _signUpValidationService.ValidateUser(createdUser, request.FullName, request.Email, request.Password,response);
            

        }
    }
}
