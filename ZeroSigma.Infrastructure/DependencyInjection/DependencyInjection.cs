﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Configuration;
using System.Text;
using ZeroSigma.Application.Authentication.Services.Encryption;
using ZeroSigma.Application.Common.Authentication;
using ZeroSigma.Application.Common.Interfaces;
using ZeroSigma.Application.Interfaces;
using ZeroSigma.Infrastructure.Authentication;
using ZeroSigma.Infrastructure.Authentication.AccessToken;
using ZeroSigma.Infrastructure.Authentication.Encryption;
using ZeroSigma.Infrastructure.Authentication.RefreshToken;
using ZeroSigma.Infrastructure.Persistance;
using ZeroSigma.Infrastructure.Persistance.Repositories.IdentityAccess;
using ZeroSigma.Infrastructure.Persistance.Repositories.Users;

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
            services.AddScoped<IUnitOfWork ,UnitOfWork>();
            services.AddScoped<IEncryptionService,EncryptionService>();
            services.AddScoped<IIdentityAccessRepository,IdentityAccessRepository>();
            services.AddScoped<IJwtTokenProcessingService,JwtTokenProcessingService>();
            services.AddScoped<IJwtTokenValidation, JwtTokenValidationService>();
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options => {

                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = "https://localhost:5001",
                    ValidIssuer = "https://localhost:5001",
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes("superSecretKey@345")
                    ),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddDbContext<ApplicationDbContext>(options=>options.UseMySQL(configuration.GetConnectionString("ZeroSigmaMySqlDb")!));
            
            return services;
            
        }
    }
}
