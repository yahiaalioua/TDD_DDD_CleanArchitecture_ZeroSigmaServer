using ZeroSigma.Domain.Entities;

namespace ZeroSigma.Application.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        User? GetByEmail(string email);

    }
}