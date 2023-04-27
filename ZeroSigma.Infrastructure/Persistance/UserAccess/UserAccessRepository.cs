using System.IdentityModel.Tokens.Jwt;
using ZeroSigma.Application.Common.Interfaces;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class UserAccessRepository : IUserAccessRepository
    {
        public static List<UserAccess> _userAccessDb=new();
        public static List<UserAccessToken> _userAccessToken = new();
        public static List<UserRefreshToken> _userRefreshToken = new();

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
    }
}