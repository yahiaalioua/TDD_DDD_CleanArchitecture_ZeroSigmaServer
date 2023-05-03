using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;

namespace ZeroSigma.Application.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetByIdAsync(UserID id);
        Task<User?> GetByEmailAsync(UserEmail email);
        Task AddUserAsync(User user);
    }
}