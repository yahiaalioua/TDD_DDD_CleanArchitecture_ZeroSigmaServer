using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Models;

namespace ZeroSigma.Domain.ValueObjects.UserAccessToken
{
    public sealed class AccessTokenID : ValueObject
    {
        public Guid Value { get; }

        private AccessTokenID(Guid value)
        {
            Value = value;
        }

        public static AccessTokenID CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        public static AccessTokenID Create(Guid value)
        {
            return new AccessTokenID(value);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
