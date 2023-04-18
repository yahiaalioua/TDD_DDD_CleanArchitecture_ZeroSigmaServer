using System.Text.Json.Serialization;

namespace ZeroSigma.Domain.Common.Errors
{
    public class Error
    {
        public List<Dictionary<string, string[]>> ?Errors { get; set; } 
    }
}