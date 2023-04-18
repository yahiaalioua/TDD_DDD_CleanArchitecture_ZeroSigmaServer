using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Common.Errors;

namespace ZeroSigma.Domain.Common.Results
{
    public class UnexpectedResult<T> : Result<T>
    {
        private CustomProblemDetails _problemDetails;
        public UnexpectedResult(CustomProblemDetails problemDetails)
        {
            _problemDetails = problemDetails;
        }
        public override ResultType ResultType => ResultType.Unexpected;

        public override CustomProblemDetails CustomProblemDetails => _problemDetails;

        public override T Data => default(T);
    }
}
