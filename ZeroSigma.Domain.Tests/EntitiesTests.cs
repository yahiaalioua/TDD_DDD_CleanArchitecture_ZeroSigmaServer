
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.User.ValueObjects;
using ZeroSigma.Domain.Validation.StructuralValidation.DomainErrors;

namespace ZeroSigma.Tests.Domain
{
    public class EntitiesTests
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
        public void ShouldReturn()
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
    }
}
