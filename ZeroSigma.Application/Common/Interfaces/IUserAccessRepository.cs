using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.User.ValueObjects;

namespace ZeroSigma.Application.Common.Interfaces
{
    public interface IUserAccessRepository
    {
        UserAccess? GetUserAccessById(UserID userID);
        void AddUserAccess(UserAccess userAccess);
        void AddUserAccessToken(UserAccessToken userAccessToken);
        void AddUserRefreshToken(UserRefreshToken userRefreshToken);
    }
}
