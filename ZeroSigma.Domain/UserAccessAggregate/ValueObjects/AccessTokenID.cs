using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Models;

namespace ZeroSigma.Domain.UserAccessAggregate.ValueObjects
{
    public sealed class AccessTokenID : ValueObject
    {
        public Guid Value { get; }

        private AccessTokenID (Guid value)
        {
            Value = value;
        }

        public static AccessTokenID CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
