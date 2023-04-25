using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.User.ValueObjects;
using ZeroSigma.Domain.UserAggregate.ValueObjects;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class UserRepository : IUserRepository
    {
        public static List<User> _userDb = new();

        public void Add(User user)
        {
            _userDb.Add(user);
            
        }

        public User? GetByEmail(UserEmail email)
        {
            var user = _userDb.FirstOrDefault(x => x.Email.Value == email.Value);
            return user;
        }
    }
}