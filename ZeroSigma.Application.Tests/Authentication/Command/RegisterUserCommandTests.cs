﻿using Moq;
using ZeroSigma.Application.Authentication.Commands;
using ZeroSigma.Application.Authentication.Services.Encryption;
using ZeroSigma.Application.Authentication.Services.ProcessingServices.SignUpProcessingServices;
using ZeroSigma.Application.Authentication.Services.ValidationServices.SignUp;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;
using ZeroSigma.Domain.Validation.StructuralValidation.DomainErrors;
using ZeroSigma.Domain.ValueObjects.User;

namespace ZeroSigma.Application.Authentication.Command
{
    public class RegisterUserCommandTests
    {
        private  Mock<IUserRepository> _userRepositoryMock;
        private readonly User _mike;
        private readonly ISignUpValidationService _signUpValidationService;
        private readonly FullName _fullName;
        private readonly UserEmail _email;
        private readonly UserPassword _password;
        private Mock<ISignUpProcessingService> _userProcessingserviceMock;
        public RegisterUserCommandTests()
        {
            _password = UserPassword.Create("UserPass2453..?").Data;
            _fullName = FullName.Create("mike").Data;
            _email = UserEmail.Create("mike@mail.com").Data;
            _userRepositoryMock = new Mock<IUserRepository>();
            _userProcessingserviceMock=new Mock<ISignUpProcessingService>();
            _mike = User.Create(_fullName, _email,_password);
            _signUpValidationService = new SignUpValidationService(
                _userProcessingserviceMock.Object,
                _userRepositoryMock.Object
                );
            
        }

        [Fact]
        public async Task Handle_ShouldReturnDuplicateEmailProblemDetailWhenUserEmailIsNotUnique()
        {
            // arrange
            var command = new RegisterCommand(_mike.FullName.Value,_mike.Email.Value,_mike.Password.Value);
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<UserEmail>())).ReturnsAsync(_mike);
            var handler = new RegisterCommandHandler(_signUpValidationService);
            //act
            Result<SignUpResponse> result= await handler.Handle(command, default);
            //assert
            _userRepositoryMock.Verify(r=>r.AddUserAsync(_mike), Times.Never);
            _userRepositoryMock.Verify(r=>r.GetByEmailAsync(It.IsAny<UserEmail>()), Times.Once);
            Assert.True(result.CustomProblemDetails==SignUpLogicalValidationErrors.DuplicateEmailError);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResultWhenUserEmailIsUniqueAndPasswordIsValid()
        {
            // arrange
            User? NonExistingUser = null;
            var command = new RegisterCommand(_mike.FullName.Value,_mike.Email.Value,_mike.Password.Value);
            string successMessage = "You successfully registered";
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<UserEmail>())).ReturnsAsync(NonExistingUser);
            _userProcessingserviceMock.Setup(x => x.CreateUser(It.IsAny<FullName>(), It.IsAny<UserEmail>(), It.IsAny<UserPassword>())).Returns(_mike);
            _userProcessingserviceMock.Setup(x => x.ProcessSignUpRequest(It.IsAny<FullName>(), It.IsAny<UserEmail>(), It.IsAny<UserPassword>())).ReturnsAsync(_mike);
            

            var handler = new RegisterCommandHandler(_signUpValidationService);
            //act
            
            Result<SignUpResponse> result = await handler.Handle(command, default);
            //assert
            _userRepositoryMock.Verify(r => r.GetByEmailAsync(It.IsAny<UserEmail>()), Times.Once);
            Assert.Equal(result.Data.Message, successMessage);
            Assert.True(result.ResultType==ResultType.Ok);
            
        }
        [Fact]
        public async Task Handle_ShouldReturnInvalidPasswordLengthErrorWhenPasswordLengthIsLessThanEightCharacters()
        {
            // arrange
            string lessThan8charsPassword = ">8char";
            var command = new RegisterCommand("username", "email@valid.com", lessThan8charsPassword);
            _userRepositoryMock.Setup(r => r.AddUserAsync(It.IsAny<User>()));
            var handler = new RegisterCommandHandler(_signUpValidationService);
            //act
            Result<SignUpResponse> result = await handler.Handle(command, default);
            //assert
            _userRepositoryMock.Verify(r => r.AddUserAsync(It.IsAny<User>()), Times.Never);
            Assert.IsAssignableFrom<Result<SignUpResponse>>(result);
            Assert.True(result.CustomProblemDetails == DomainErrors.InvalidPasswordLengthError);

        }
        [Fact]
        public async Task Handle_ShouldReturnInvalidPasswordErrorWhenPasswordItsNotSecureEnough()
        {
            // arrange
            string notSecurePassword = "eightCharsPassword";
            var command = new RegisterCommand("username","email@valid.com",notSecurePassword);
            _userRepositoryMock.Setup(r => r.AddUserAsync(It.IsAny<User>()));
            var handler = new RegisterCommandHandler(_signUpValidationService);
            //act
            Result<SignUpResponse> result = await handler.Handle(command, default);
            //assert
            _userRepositoryMock.Verify(r => r.AddUserAsync(It.IsAny<User>()), Times.Never);
            Assert.IsAssignableFrom<Result<SignUpResponse>>(result);
            Assert.True(result.CustomProblemDetails == DomainErrors.InvalidPasswordError);

        }

        [Fact]
        public async Task Handle_ShouldReturnMissingSpecialCharacterErrorWhenPasswordDoesNotContainsAtLeastOneSpecialCharacter()
        {
            // arrange
            string passwordWithoutSpecialChar = "missingSpecialchars0";
            var command = new RegisterCommand("username", "email@valid.com", passwordWithoutSpecialChar);
            var handler = new RegisterCommandHandler(_signUpValidationService);
            //act
            _userRepositoryMock.Setup(r => r.AddUserAsync(It.IsAny<User>()));
            Result<SignUpResponse> result = await handler.Handle(command, default);
            //assert
            _userRepositoryMock.Verify(r => r.AddUserAsync(It.IsAny<User>()), Times.Never);
            Assert.IsAssignableFrom<Result<SignUpResponse>>(result);
            Assert.True(result.CustomProblemDetails == DomainErrors.MissingSpecialCharacterError);

        }

        [Fact]
        public async Task Handle_ShouldReturnInvalidEmailAddressErrorWhenEmailIsNotAValidEmail()
        {
            // arrange
            string invalidEmail = "mail@invalid";
            User? user = null;
            var command = new RegisterCommand("userName",invalidEmail,"TestPassword4444.");
            var handler = new RegisterCommandHandler(_signUpValidationService);
            //act
            _userRepositoryMock.Setup(r => r.GetByEmailAsync(It.IsAny<UserEmail>())).ReturnsAsync(user);
            _userRepositoryMock.Setup(r => r.AddUserAsync(It.IsAny<User>()));
            Result<SignUpResponse> result = await handler.Handle(command, default);
            //assert
            _userRepositoryMock.Verify(r => r.GetByEmailAsync(It.IsAny<UserEmail>()), Times.Never);
            _userRepositoryMock.Verify(r => r.AddUserAsync(It.IsAny<User>()), Times.Never);
            Assert.IsAssignableFrom<Result<SignUpResponse>>(result);
            Assert.Equal(DomainErrors.InvalidEmailAddressError,result.CustomProblemDetails);
        }
    }
}
