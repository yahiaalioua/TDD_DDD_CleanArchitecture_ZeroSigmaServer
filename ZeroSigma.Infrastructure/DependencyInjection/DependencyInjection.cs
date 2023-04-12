using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Infrastructure.Authentication;
using ZeroSigma.Infrastructure.Authentication.AccessToken;


namespace ZeroSigma.Infrastructure.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
        {
            services.Configure<AccessTokenOptions>(configuration.GetSection(nameof(AccessTokenOptions)));
            services.AddScoped<IAccessTokenProvider, AccessTokenProvider>();
            services.AddScoped<IJwtGenerator, JwtGenrator>();
            return services;
            
        }
    }
}
