using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Domain.ValueObjects.UserAccessToken;
using ZeroSigma.Domain.ValueObjects.UserRefreshToken;

namespace ZeroSigma.Infrastructure.Persistance.Repositories.IdentityAccess
{
    public interface IIdentityAccessRepository
    {
        Task AddUserAccessAsync(UserAccess userAccess);
        Task AddUserAccessBlacklistAsync(UserAccessBlackList userAccessBlackList);
        Task AddUserAccessTokenAsync(UserAccessToken userAccessToken);
        Task AddUserRefreshTokenAsync(UserRefreshToken userRefreshToken);
        Task<UserAccessBlackList?> GetUserAccessBlacklistAsync(RefreshTokenID refreshTokenID);
        Task<UserAccess?> GetUserAccessByUserId(UserID userID);
        Task<UserAccessToken?> GetUserAccessTokenByIdAsync(AccessTokenID id);
        Task UpdateUserAccessToken(UserAccessToken userAccessToken);
        Task<UserRefreshToken?> GetUserRefreshTokenByIdAsync(RefreshTokenID id);
        Task UpdateUserRefreshToken(UserRefreshToken userRefreshToken);
        Task DeleteRefreshTokenByIdAsync(RefreshTokenID refreshTokenID);
    }
}