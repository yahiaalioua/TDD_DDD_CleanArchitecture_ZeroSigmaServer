using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.Entities;

namespace ZeroSigma.Application.Authentication.Services.ProcessingServices.NewSessionProcessingServices
{
    public interface INewSessionProcessingService
    {
        void RevokeRefreshTokenAndAddToBlackList(UserRefreshToken storedRefreshToken, UserAccessBlackList userAccessBlackList);
        Task<string> RevokeAndRotateRefreshToken(UserRefreshToken storedRefreshToken, Guid id, string email, UserAccessBlackList userAccessBlackList);
        void RemoveOldRefreshToken(UserAccessBlackList userAccessBlackList);
        Task<Result<NewSessionResponse>> ProcessNewSession(NewSessionRequest request);



    }
}