using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Entities;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class UserRepository : IUserRepository
    {
        private readonly List<User> _userDb;

        public UserRepository(List<User> userDb)
        {
            _userDb = userDb;
        }

        public void Add(User user)
        {
            _userDb.Add(user);
            
        }

        public User? GetByEmail(string email)
        {
            var user = _userDb.SingleOrDefault(x => x.Email == email);
            return user;
        }
    }
}