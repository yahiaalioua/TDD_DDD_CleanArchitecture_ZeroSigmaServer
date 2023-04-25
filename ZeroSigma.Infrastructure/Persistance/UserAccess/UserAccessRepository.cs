using ZeroSigma.Application.Common.Interfaces;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.UserAggregate.ValueObjects;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class UserAccessRepository : IUserAccessRepository
    {
        public static List<UserAccess> _userAccessDb=new();

        public void Add(UserAccess userAccess)
        {
            _userAccessDb.Add(userAccess);
            
        }
    }
}