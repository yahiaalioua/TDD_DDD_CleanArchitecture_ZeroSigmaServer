using ZeroSigma.Domain.Models;

namespace ZeroSigma.Domain.ValueObjects.UserAccess
{
    public sealed class UserAccessID : ValueObject
    {
        public Guid Value { get; }

        private UserAccessID(Guid value)
        {
            Value = value;
        }

        public static UserAccessID CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static UserAccessID Create(Guid value)
        {
            return new UserAccessID(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}