using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.UserAggregate.ValueObjects;

namespace ZeroSigma.Application.Interfaces
{
    public interface IUserRepository
    {
        void Add(User user);
        User? GetByEmail(UserEmail email);

    }
}