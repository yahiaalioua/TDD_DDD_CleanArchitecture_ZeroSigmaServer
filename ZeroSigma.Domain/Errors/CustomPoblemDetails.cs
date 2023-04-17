using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZeroSigma.Domain.Errors
{
    public class CustomProblemDetails
    {
        public int Status { get; set; }
        public string Type { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string Detail { get; set; } = null!;
        public int Code { get; set; }
        public Error? Errors { get; set; }

    }
}
