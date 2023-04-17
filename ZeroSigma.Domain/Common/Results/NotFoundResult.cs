using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Errors;

namespace ZeroSigma.Domain.Common.Results
{
    public class NotFoundResults<T> : Result<T>
    {
        private CustomProblemDetails _problemDetails;
        public NotFoundResults(CustomProblemDetails problemDetails)
        {
            _problemDetails = problemDetails;
        }
        public override ResultType ResultType => ResultType.NotFound;

        public override CustomProblemDetails CustomProblemDetails => _problemDetails;

        public override T Data => default(T);
    }
}
