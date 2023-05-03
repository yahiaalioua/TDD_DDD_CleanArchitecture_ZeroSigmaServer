using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Models;

namespace ZeroSigma.Domain.ValueObjects.UserAccessBlackList
{
    public sealed class BlackListId : ValueObject
    {
        private BlackListId(Guid value)
        {
            Value = value;
        }

        public Guid Value { get; }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public static BlackListId Create(Guid value)
        {
            return new BlackListId(value);
        }
        public static BlackListId CreateUnique()
        {
            return new BlackListId(Guid.NewGuid());
        }
    }
}
