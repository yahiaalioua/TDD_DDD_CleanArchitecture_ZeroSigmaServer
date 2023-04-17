using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Errors;

namespace ZeroSigma.Domain.Common.Results
{
    public class SuccessResult<T> : Result<T>
    {
        private CustomProblemDetails _problemDetails;
        private readonly T _data;

        public SuccessResult(T data)
        {

            _data = data;
        }
        public override ResultType ResultType => ResultType.Ok;
        public override CustomProblemDetails CustomProblemDetails => _problemDetails;
        public override T Data => _data;
    }
}
