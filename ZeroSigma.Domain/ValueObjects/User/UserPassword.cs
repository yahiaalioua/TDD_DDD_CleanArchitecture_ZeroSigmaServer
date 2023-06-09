﻿using System.Text.RegularExpressions;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Models;
using ZeroSigma.Domain.Validation.StructuralValidation.DomainErrors;

namespace ZeroSigma.Domain.ValueObjects.User
{
    public sealed class UserPassword : ValueObject
    {
        public const int MinLength = 8;
        public const int MaxLength = 70;
        public string Value { get; }

        private UserPassword(string value)
        {
            Value = value;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
        public static Result<UserPassword> Create(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
            {
                return new InvalidResult<UserPassword>(DomainErrors.EmptyPasswordError);
            }
            if (password.Length < MinLength)
            {
                return new InvalidResult<UserPassword>(DomainErrors.InvalidPasswordLengthError);
            }
            if (password.Length > 9 && password.Length > MaxLength)
            {
                return new InvalidResult<UserPassword>(DomainErrors.InvalidPasswordLengthError);
            }
            if (!(Regex.IsMatch(password, "[a-z]") && Regex.IsMatch(password, "[A-Z]") && Regex.IsMatch(password, "[0-9]")))
            {
                return new InvalidResult<UserPassword>(DomainErrors.InvalidPasswordError);
            }
            if (!Regex.IsMatch(password, "[`,~,!,@,#,$,%,^,&,*,(,),_,-,+,=,{,[,},},|,\\,:,;,\",',<,,,>,.,?,/]"))
            {
                return new InvalidResult<UserPassword>(DomainErrors.MissingSpecialCharacterError);
            }
            return new SuccessResult<UserPassword>(new UserPassword(password));
        }
    }
}
