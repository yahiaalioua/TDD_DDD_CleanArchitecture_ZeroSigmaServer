using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;

namespace ZeroSigma.Application.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        User? GetByEmail(UserEmail email);

    }
}