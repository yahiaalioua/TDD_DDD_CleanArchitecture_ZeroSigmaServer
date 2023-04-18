using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using ZeroSigma.Domain.Validation;

namespace ZeroSigma.Domain.Common.Errors
{
    public class CustomProblemDetails
    {
        public HttpStatusCode Status { get; set; }
        public string Type { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Detail { get; set; } = null!;
        public string Code { get; set; }=null!;
        public Error? Errors { get; set; }

    }
}
