namespace ZeroSigma.Domain.Errors
{
    public class Error
    {
        public List<Dictionary<string, string[]>> Errors { get; set; } = null!;
    }
}