using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Authentication.Commands;
using ZeroSigma.Application.Authentication.Services.ValidationServices.SignUp;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;

namespace ZeroSigma.Application.Authentication.Command
{
    public class RegisterUserCommandTests
    {
        private  Mock<IUserRepository> _userRepositoryMock;
        private readonly User _mike;
        private readonly ISignUpValidationService _signUpValidationService;
        public RegisterUserCommandTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _mike = new User()
            {
                FullName="Mike", Email="mike@mail.com",Password="All0wedPassword1.",
                AccessToken="accessToken", RefreshToken="refreshToken"
            };
            _signUpValidationService = new SignUpValidationService(_userRepositoryMock.Object);
            
        }

        [Fact]
        public async Task Handle_ShouldReturnDuplicateEmailProblemDetailWhenUserEmailIsNotUnique()
        {
            // arrange
            var command = new RegisterCommand(_mike.FullName,_mike.Email,_mike.Password);
            _userRepositoryMock.Setup(r => r.GetByEmail(_mike.Email)).Returns(_mike);
            var handler = new RegisterCommandHandler(_signUpValidationService);
            //act
            Result<SignUpResponse> result= await handler.Handle(command, default);
            //assert
            _userRepositoryMock.Verify(r=>r.Add(_mike), Times.Never);
            _userRepositoryMock.Verify(r=>r.GetByEmail(_mike.Email), Times.Once);
            Assert.True(result.CustomProblemDetails==SignUpLogicalValidationErrors.DuplicateEmailError);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResultWhenUserEmailIsUniqueAndPasswordIsValid()
        {
            // arrange
            User? NonExistingUser = null;
            var command = new RegisterCommand(_mike.FullName, _mike.Email,_mike.Password);
            _userRepositoryMock.Setup(r => r.GetByEmail(_mike.Email)).Returns(NonExistingUser);
            _userRepositoryMock.Setup(r => r.Add(_mike));
            var handler = new RegisterCommandHandler(_signUpValidationService);
            //act
            Result<SignUpResponse> result = await handler.Handle(command, default);
            //assert
            _userRepositoryMock.Verify(x => x.Add(It.IsAny<User>()), Times.Once);
            _userRepositoryMock.Verify(r => r.GetByEmail(_mike.Email), Times.Once);
            
            Assert.IsAssignableFrom<Result<SignUpResponse>>(result);
            Assert.True(result.ResultType == ResultType.Ok);
            
        }
        [Fact]
        public async Task Handle_ShouldReturnInvalidPasswordLengthErrorWhenPasswordLengthIsLessThanEightCharacters()
        {
            // arrange
            User? user = null;
            var command = new RegisterCommand(_mike.FullName, _mike.Email, ">8char");
            _userRepositoryMock.Setup(r => r.GetByEmail(_mike.Email)).Returns(user);
            _userRepositoryMock.Setup(r => r.Add(_mike));
            var handler = new RegisterCommandHandler(_signUpValidationService);
            //act
            Result<SignUpResponse> result = await handler.Handle(command, default);
            //assert
            _userRepositoryMock.Verify(r => r.GetByEmail(_mike.Email), Times.Once);
            _userRepositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
            Assert.IsAssignableFrom<Result<SignUpResponse>>(result);
            Assert.True(result.CustomProblemDetails == SignUpStructuralValidationErrors.InvalidPasswordLengthError);

        }
        [Fact]
        public async Task Handle_ShouldReturnInvalidPasswordErrorWhenPasswordItsNotSecureEnough()
        {
            // arrange
            User? user = null;
            var command = new RegisterCommand(_mike.FullName, _mike.Email, "eightCharsPassword");
            _userRepositoryMock.Setup(r => r.GetByEmail(_mike.Email)).Returns(user);
            _userRepositoryMock.Setup(r => r.Add(_mike));
            var handler = new RegisterCommandHandler(_signUpValidationService);
            //act
            Result<SignUpResponse> result = await handler.Handle(command, default);
            //assert
            _userRepositoryMock.Verify(r => r.GetByEmail(_mike.Email), Times.Once);
            _userRepositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
            Assert.IsAssignableFrom<Result<SignUpResponse>>(result);
            Assert.True(result.CustomProblemDetails == SignUpStructuralValidationErrors.InvalidPasswordError);

        }

        [Fact]
        public async Task Handle_ShouldReturnMissingSpecialCharacterErrorWhenPasswordDoesNotContainsAtLeastOneSpecialCharacter()
        {
            // arrange
            User? user = null;
            var command = new RegisterCommand(_mike.FullName, _mike.Email, "eightCharsPassword12");
            _userRepositoryMock.Setup(r => r.GetByEmail(_mike.Email)).Returns(user);
            _userRepositoryMock.Setup(r => r.Add(_mike));
            var handler = new RegisterCommandHandler(_signUpValidationService);
            //act
            Result<SignUpResponse> result = await handler.Handle(command, default);
            //assert
            _userRepositoryMock.Verify(r => r.GetByEmail(_mike.Email), Times.Once);
            _userRepositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
            Assert.IsAssignableFrom<Result<SignUpResponse>>(result);
            Assert.True(result.CustomProblemDetails == SignUpStructuralValidationErrors.MissingSpecialCharacterError);

        }

        [Fact]
        public async Task Handle_ShouldReturnInvalidEmailAddressErrorWhenEmailIsNotAValidEmail()
        {
            // arrange
            User? user = null;
            var command = new RegisterCommand(_mike.FullName,"invalidEmailAddress","TestPassword4444.");
            _userRepositoryMock.Setup(r => r.GetByEmail("invalidEmailAddress")).Returns(user);
            var handler = new RegisterCommandHandler(_signUpValidationService);
            //act
            Result<SignUpResponse> result = await handler.Handle(command, default);
            //assert
            _userRepositoryMock.Verify(r => r.GetByEmail("invalidEmailAddress"), Times.Never);
            _userRepositoryMock.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
            Assert.IsAssignableFrom<Result<SignUpResponse>>(result);
            Assert.True(result.CustomProblemDetails == SignUpStructuralValidationErrors.InvalidEmailAddressError);
        }
    }
}
