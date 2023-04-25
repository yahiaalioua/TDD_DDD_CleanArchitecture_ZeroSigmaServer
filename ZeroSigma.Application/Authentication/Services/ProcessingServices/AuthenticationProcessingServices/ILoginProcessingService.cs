using System.IdentityModel.Tokens.Jwt;
using ZeroSigma.Domain.Entities;

namespace ZeroSigma.Application.Authentication.Services.ProcessingServices.AuthenticationProcessingServices
{
    public interface ILoginProcessingService
    {
        JwtSecurityToken DecodeJwt(string token);
        DateTime GetTokenExpiryDate(string token);
        DateTime GetTokenIssueDate(string token);
        string ProcessAuthentication(User user);
    }
}