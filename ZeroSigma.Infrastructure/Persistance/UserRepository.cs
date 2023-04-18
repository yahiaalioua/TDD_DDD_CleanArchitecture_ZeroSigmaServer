using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Entities;

namespace ZeroSigma.Infrastructure.Persistance
{
    public class UserRepository : IUserRepository
    {
        public static List<User> _userDb=new();

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