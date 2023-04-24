﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Validation.LogicalValidation.Errors.Authentication;
using ZeroSigma.Domain.Validation.StructuralValidation.DomainErrors;

namespace ZeroSigma.Domain.User.ValueObjects
{
    public sealed class FullName
    {
        public const int MaxLength = 50;

        private FullName(string value)
        {
            Value = value;
        }

        public string Value { get; } 

        public static Result<FullName> Create(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                return new InvalidResult<FullName>(DomainErrors.EmptyFullNameError);
            }
            if (fullName.Length > MaxLength)
            {
                return new InvalidResult<FullName>(DomainErrors.InvalidFullNameLengthError);
            }
            return new SuccessResult<FullName>(new FullName(fullName));
        }

    }
}