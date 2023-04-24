using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeroSigma.Application.Authentication.Services.ValidationServices.Login;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.DTO.Authentication;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Domain.Common.Errors;
using ZeroSigma.Domain.Common.Results;
using ZeroSigma.Domain.User.ValueObjects;

namespace ZeroSigma.Application.Authentication.Queries
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<AuthenticationResponse>>
    {
        private readonly IAccessTokenProvider _accessTokenProvider;
        private readonly IRefreshTokenProvider _refreshTokenProvider;
        private readonly ILoginValidationService _loginValidationService;
        private readonly IUserRepository _userRepository;

        public LoginQueryHandler(
            IAccessTokenProvider accessTokenProvider,
            IRefreshTokenProvider refreshTokenProvider
,
            ILoginValidationService loginValidationService,
            IUserRepository userRepository)
        {
            _accessTokenProvider = accessTokenProvider;
            _refreshTokenProvider = refreshTokenProvider;
            _loginValidationService = loginValidationService;
            _userRepository = userRepository;
        }

        public async Task<Result<AuthenticationResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user=_userRepository.GetByEmail(request.Email);
            var accessToken = "";
            var refreshToken = "";
            if (user != null)
            {
                var fullNameResult = FullName.Create(user.FullName.Value);
                var userId = UserID.CreateUnique().Value;
                accessToken = _accessTokenProvider.GenerateAccessToken(userId, fullNameResult.Data.Value, user.Email);
                refreshToken = _refreshTokenProvider.GenerateRefreshToken(userId,user.Email);
            }
            return _loginValidationService.ValidateUser(user,request.Email, request.Password, accessToken, refreshToken);          
            
        }
    }
}
