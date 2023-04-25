
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.User.ValueObjects;
using ZeroSigma.Domain.UserAggregate.ValueObjects;
using ZeroSigma.Domain.Validation.StructuralValidation.DomainErrors;

namespace ZeroSigma.Tests.Domain
{
    public class UserEntityTests
    {
        [Fact]
        public void ShouldReturnInvalidErrorWhenFullNameIsNullOrEmpty()
        {
            //arrange
            var fullName = " ";

            //act
            var createdFullName= FullName.Create(fullName);
            //assert
            Assert.Equal(DomainErrors.EmptyFullNameError, createdFullName.CustomProblemDetails);
            Assert.Null(createdFullName.Data);
            Assert.True(createdFullName.ResultType==ResultType.Invalid);
        }
        [Fact]
        public void ShouldReturnInvalidErrorWhenFullNameIsMoreThan50Characters()
        {
            //arrange
            var fullName = "qgkxnoukmoumawcholewgfdtroefraevrtkirbrlvqwgjdnxpouczsq";

            //act
            var createdFullName = FullName.Create(fullName);
            //assert
            Assert.Equal(DomainErrors.InvalidFullNameLengthError, createdFullName.CustomProblemDetails);
            Assert.Null(createdFullName.Data);
            Assert.True(createdFullName.ResultType == ResultType.Invalid);
        }
        [Fact]
        public void ShouldReturnResultTypeOKWhenFullNameIsValid()
        {
            //arrange
            var fullName = "My Name";

            //act
            var createdFullName = FullName.Create(fullName);
            //assert
            Assert.Equal(fullName,createdFullName.Data.Value);
            Assert.Null(createdFullName.CustomProblemDetails);
            Assert.NotNull(createdFullName.Data);
            Assert.True(createdFullName.ResultType == ResultType.Ok);
        }
        [Fact]
        public void ShouldReturnEmptyEmailErrorWhenEmailIsNullOrEmpty()
        {
            //arrange
            var email = " ";

            //act
            var createdEmail = UserEmail.Create(email);
            //assert
            Assert.Equal(DomainErrors.EmptyEmailError, createdEmail.CustomProblemDetails);
            Assert.Null(createdEmail.Data);
            Assert.True(createdEmail.ResultType == ResultType.Invalid);
        }
        [Fact]
        public void ShouldReturnInvalidEmailAddressErrorWhenEmailIsNotAValidEmail()
        {
            //arrange
            var email = "invalidEmail@emailMissedValues";

            //act
            var createdEmail = UserEmail.Create(email);
            //assert
            Assert.Equal(DomainErrors.InvalidEmailAddressError, createdEmail.CustomProblemDetails);
            Assert.Null(createdEmail.Data);
            Assert.True(createdEmail.ResultType == ResultType.Invalid);
        }
        [Fact]
        public void ShouldReturnSuccessResultWhenEmailIsValid()
        {
            //arrange
            var email = "valid@mail.valid";

            //act
            var createdEmail = UserEmail.Create(email);
            //assert
            Assert.NotNull(createdEmail.Data);
            Assert.True(createdEmail.ResultType == ResultType.Ok);
            Assert.Equal(email, createdEmail.Data.Value);
        }
        [Fact]
        public void ShouldReturnInvalidErrorWhenPasswordIsNullOrEmpty()
        {
            //arrange
            var password = "";

            //act
            var createdPassword = UserPassword.Create(password);
            //assert
            Assert.Equal(DomainErrors.EmptyPasswordError, createdPassword.CustomProblemDetails);
            Assert.Null(createdPassword.Data);
            Assert.True(createdPassword.ResultType == ResultType.Invalid);
        }

    }
}
