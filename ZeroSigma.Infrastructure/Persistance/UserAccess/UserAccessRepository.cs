using System.IdentityModel.Tokens.Jwt;
using ZeroSigma.Application.Common.Interfaces;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;
using ZeroSigma.Domain.ValueObjects.UserRefreshToken;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class UserAccessRepository : IUserAccessRepository
    {
        public static List<UserAccess> _userAccessDb=new();
        public static List<UserAccessToken> _userAccessToken = new();
        public static List<UserRefreshToken> _userRefreshToken = new();
        public static List<UserAccessBlackList> _userAccessBlackList = new();

        public UserAccess? GetUserAccessById(UserID userID)
        {
            return _userAccessDb.SingleOrDefault(x => x.UserID == userID);
        }
        public void AddUserAccess(UserAccess userAccess)
        {
            _userAccessDb.Add(userAccess);
        }
        public void AddUserAccessToken(UserAccessToken userAccessToken)
        {
            _userAccessToken.Add(userAccessToken);
        }
        public void AddUserRefreshToken(UserRefreshToken userRefreshToken)
        {
            _userRefreshToken.Add(userRefreshToken);
        }
        public UserRefreshToken? GetUserRefreshToken(string refreshToken)
        {
            return _userRefreshToken.FirstOrDefault(x=>x.RefreshToken == refreshToken);
        }
        public UserRefreshToken? GetUserRefreshTokenByUserID(Guid userID)
        {
            return _userRefreshToken.FirstOrDefault(x => x.userID.Value==userID);
        }
        public void DeleteRefreshToken(RefreshTokenID refreshTokenID)
        {
            var index = _userRefreshToken.FindIndex(r => r.Id == refreshTokenID);
            _userRefreshToken.RemoveAt(index);
        }
        public void UpdateRefreshToken(RefreshTokenID refreshTokenID, string refreshToken)
        {
            var index = _userRefreshToken.FindIndex(r => r.Id ==refreshTokenID);

            if (index != -1)
            {
                _userRefreshToken[index].RefreshToken = refreshToken;
            }
        }
        public void AddUserAccessBlacklist(UserAccessBlackList userAccessBlackList)
        {
            _userAccessBlackList.Add(userAccessBlackList);
        }
        public UserAccessBlackList? GetUserAccessBlackList(RefreshTokenID refreshTokenID)
        {
            return _userAccessBlackList.FirstOrDefault(x => x.RefreshTokenID == refreshTokenID);
        }
    }
}