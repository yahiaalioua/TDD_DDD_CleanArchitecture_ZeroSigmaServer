using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Models;

namespace ZeroSigma.Domain.ValueObjects.UserRefreshToken
{
    public sealed class RefreshTokenID : ValueObject
    {
        public Guid Value { get; }

        private RefreshTokenID(Guid value)
        {
            Value = value;
        }

        public static RefreshTokenID CreateUnique()
        {
            return new(Guid.NewGuid());
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
