
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.User.ValueObjects;
using ZeroSigma.Domain.Validation.StructuralValidation.DomainErrors;

namespace ZeroSigma.Tests.Domain
{
    public class EntitiesTests
    {
        [Fact]
        public void ShouldReturnInvalidErrorWhenIsNullOrEmpty()
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
    }
}
