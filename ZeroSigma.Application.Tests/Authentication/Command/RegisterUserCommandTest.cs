using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Authentication.Commands;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.Validation.StructuralValidation.Errors.Authentication;

namespace ZeroSigma.Application.Authentication.Command
{
    public class RegisterUserCommandTest
    {
        private  Mock<IUserRepository> _userRepositoryMock;
        private readonly User _mike;
        public RegisterUserCommandTest()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mike = new User()
            {
                FullName="Mike", Email="mike@mail.com",Password="passRandom",
                AccessToken="accessToken", RefreshToken="refreshToken"
            };
            
        }

        [Fact]
        public async Task Handle_ShouldReturnDuplicateEmailProblemDetailWhenUserEmailIsNotUnique()
        {
            // arrange
            var command = new RegisterCommand(_mike.FullName,_mike.Email,_mike.Password);
            _userRepositoryMock.Setup(r => r.GetByEmail(_mike.Email)).Returns(_mike);
            var handler = new RegisterCommandHandler(_userRepositoryMock.Object);
            //act
            Result<SignUpResponse> result= await handler.Handle(command, default);
            //assert
            _userRepositoryMock.Verify(r=>r.Add(_mike), Times.Never);
            _userRepositoryMock.Verify(r=>r.GetByEmail(_mike.Email), Times.Once);
            Assert.True(result.CustomProblemDetails==SignUpStructuralValidationErrors.DuplicateEmailError);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResultWhenUserEmailIsUnique()
        {
            // arrange
            User? user = null;
            var command = new RegisterCommand(_mike.FullName, _mike.Email, _mike.Password);
            _userRepositoryMock.Setup(r => r.GetByEmail(_mike.Email)).Returns(user);
            _userRepositoryMock.Setup(r => r.Add(_mike));
            var handler = new RegisterCommandHandler(_userRepositoryMock.Object);
            //act
            Result<SignUpResponse> result = await handler.Handle(command, default);
            //assert
            _userRepositoryMock.Verify(r => r.GetByEmail(_mike.Email), Times.Once);
            Assert.IsAssignableFrom<Result<SignUpResponse>>(result);
            Assert.True(result.ResultType == ResultType.Ok);
            
        }
    }
}
