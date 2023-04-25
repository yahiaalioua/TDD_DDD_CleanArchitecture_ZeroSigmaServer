using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Authentication.Services.Encryption;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Entities;
using ZeroSigma.Domain.User.ValueObjects;
using ZeroSigma.Domain.UserAggregate.ValueObjects;

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
        public User? ProcessSignUpRequest(FullName fullname, UserEmail email, UserPassword password)
        {
            var user = CreateUser(fullname, email, password);
            if (user is not null)
            {
                string encryptedPassword = _encryptionService.EncryptPassword(user.Password.Value);
                user.Password = UserPassword.Create(encryptedPassword).Data;
                _userRepository.Add(user);
            }
            return user;
        }
    }
}
