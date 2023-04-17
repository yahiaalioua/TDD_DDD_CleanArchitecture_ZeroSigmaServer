using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Errors;

namespace ZeroSigma.Domain.Common.Results
{
    public class InvalidResult<T> : Result<T>
    {
        private CustomProblemDetails _problemDetails;
        public InvalidResult(CustomProblemDetails problemDetails)
        {
            _problemDetails = problemDetails;
        }
        public override ResultType ResultType => ResultType.Invalid;

        public override CustomProblemDetails CustomProblemDetails => _problemDetails;

        public override T Data => default(T);
    }
}
