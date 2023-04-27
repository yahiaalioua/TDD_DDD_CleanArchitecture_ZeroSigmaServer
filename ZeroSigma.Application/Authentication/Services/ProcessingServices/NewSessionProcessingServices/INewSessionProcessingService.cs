using System.IdentityModel.Tokens.Jwt;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Common.Results;

namespace ZeroSigma.Application.Authentication.Services.ProcessingServices.NewSessionProcessingServices
{
    public interface INewSessionProcessingService
    {
        Result<string> ProcessNewSession(NewSessionRequest request);
    }
}