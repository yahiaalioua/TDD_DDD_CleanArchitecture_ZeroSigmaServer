using ZeroSigma.Domain.Models;

namespace ZeroSigma.Domain.User.ValueObjects
{
    public sealed class UserID : ValueObject
    {
        public Guid Value { get;}

        private UserID(Guid value)
        {
            Value = value;
        }

        public static UserID CreateUnique()
        {
            return new (Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}