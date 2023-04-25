using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZeroSigma.Application.Authentication.Services.Encryption;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.Common.Interfaces;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Infrastructure.Authentication;
using ZeroSigma.Infrastructure.Authentication.AccessToken;
using ZeroSigma.Infrastructure.Authentication.Encryption;
using ZeroSigma.Infrastructure.Authentication.RefreshToken;
using ZeroSigma.Infrastructure.Persistance;

namespace ZeroSigma.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<AccessTokenOptions>(configuration.GetSection(nameof(AccessTokenOptions)));
            services.Configure<RefreshTokenOptions>(configuration.GetSection(nameof(RefreshTokenOptions)));
            services.AddScoped<IAccessTokenProvider, AccessTokenProvider>();
            services.AddScoped<IRefreshTokenProvider,RefreshTokenProvider>();
            services.AddScoped<IJwtGenerator, JwtGenrator>();
            services.AddScoped<IUserRepository,UserRepository>();
            services.AddScoped<IEncryptionService,EncryptionService>();
            services.AddScoped<IUserAccessRepository,UserAccessRepository>();
            return services;
            
        }
    }
}
