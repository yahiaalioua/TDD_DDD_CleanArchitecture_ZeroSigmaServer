using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Domain.ValueObjects.UserRefreshToken;

namespace ZeroSigma.Application.Common.Interfaces
{
    public interface IUserAccessRepository
    {
        UserAccess? GetUserAccessById(UserID userID);
        void AddUserAccess(UserAccess userAccess);
        void AddUserAccessToken(UserAccessToken userAccessToken);
        void AddUserRefreshToken(UserRefreshToken userRefreshToken);
        UserRefreshToken? GetUserRefreshToken(string refreshToken);
        void UpdateRefreshToken(RefreshTokenID refreshTokenID, string refreshToken);
        void AddUserAccessBlacklist(UserAccessBlackList userAccessBlackList);
        void DeleteRefreshToken(RefreshTokenID refreshTokenID);
        UserAccessBlackList? GetUserAccessBlackList(RefreshTokenID refreshTokenID);
        UserRefreshToken? GetUserRefreshTokenByUserID(Guid userID);
    }
}
