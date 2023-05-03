using ZeroSigma.Application.Authentication.Services.Encryption;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.ValueObjects.User;

namespace ZeroSigma.Application.Authentication.Services.ProcessingServices.SignUpProcessingServices
{
    public class SignUpProcessingService : ISignUpProcessingService
    {
        private readonly IEncryptionService _encryptionService;
        private readonly IUserRepository _userRepository;

        public SignUpProcessingService(
            IEncryptionService encryptionService,
            IUserRepository userRepository
            )
        {
            _encryptionService = encryptionService;
            _userRepository = userRepository;
        }

        public User CreateUser(FullName fullname, UserEmail email, UserPassword password)
        {
            User createdUser = User.Create(fullname, email, password);
            return createdUser;
        }
        public async Task<User?> ProcessSignUpRequest(FullName fullname, UserEmail email, UserPassword password)
        {
            string encryptedPassword = _encryptionService.EncryptPassword(password.Value);
            var user = CreateUser(fullname, email, UserPassword.Create(encryptedPassword).Data);
            if (user is not null)
            {
                await _userRepository.AddUserAsync(user);
            }
            return user;
        }
    }
}
