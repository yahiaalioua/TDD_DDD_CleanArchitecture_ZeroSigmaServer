using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Common.Errors;

namespace ZeroSigma.Domain.Common.Results
{
    public class UnauthorizedResults<T> : Result<T>
    {
        private CustomProblemDetails _problemDetails;
        public UnauthorizedResults(CustomProblemDetails problemDetails)
        {
            _problemDetails = problemDetails;
        }
        public override ResultType ResultType => ResultType.Unauthorized;

        public override CustomProblemDetails CustomProblemDetails => _problemDetails;

        public override T Data => default(T);
    }
}
