using ZeroSigma.Domain.Models;

namespace ZeroSigma.Domain.UserAccessAggregate.ValueObjects
{
    public sealed class UserAccessID : ValueObject
    {
        public Guid Value { get;}

        private UserAccessID(Guid value)
        {
            Value = value;
        }

        public static UserAccessID CreateUnique()
        {
            return new (Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}