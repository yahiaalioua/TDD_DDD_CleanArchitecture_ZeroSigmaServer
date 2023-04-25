using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Models;
using ZeroSigma.Domain.Validation.StructuralValidation.DomainErrors;

namespace ZeroSigma.Domain.UserAggregate.ValueObjects
{
    public sealed class UserEmail 
    {
        public string Value { get; }

        private UserEmail(string value)
        {
            Value = value;
        }

       

        private static bool IsValidEmail(string email)
        {
            string emailTrimed = email.Trim();

            if (!string.IsNullOrEmpty(emailTrimed))
            {
                bool hasWhitespace = emailTrimed.Contains(" ");

                int indexOfAtSign = emailTrimed.LastIndexOf('@');

                if (indexOfAtSign > 0 && !hasWhitespace)
                {
                    string afterAtSign = emailTrimed.Substring(indexOfAtSign + 1);

                    int indexOfDotAfterAtSign = afterAtSign.LastIndexOf('.');

                    if (indexOfDotAfterAtSign > 0 && afterAtSign.Substring(indexOfDotAfterAtSign).Length > 1)
                        return true;
                }
            }
            return false;
        }

        public static Result<UserEmail> Create(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return new InvalidResult<UserEmail>(DomainErrors.EmptyEmailError);
            }
            if (!IsValidEmail(email))
            {
                return new InvalidResult<UserEmail>(DomainErrors.InvalidEmailAddressError);
            }
            return new SuccessResult<UserEmail>(new UserEmail(email));
        }
    }
}
