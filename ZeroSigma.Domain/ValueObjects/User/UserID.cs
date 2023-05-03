using ZeroSigma.Domain.Models;

namespace ZeroSigma.Domain.ValueObjects.User
{
    public sealed class UserID : ValueObject
    {
        public Guid Value { get; }

        private UserID(Guid value)
        {
            Value = value;
        }

        public static UserID CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static UserID Create(Guid value)
        {
            return new UserID(value);
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}