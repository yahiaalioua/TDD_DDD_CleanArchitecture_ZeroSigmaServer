﻿using Microsoft.Extensions.DependencyInjection;
using ZeroSigma.Application.Authentication.Services.Encryption;
using ZeroSigma.Application.Authentication.Services.ProcessingServices.AuthenticationProcessingServices;
using ZeroSigma.Application.Authentication.Services.ProcessingServices.NewSessionProcessingServices;
using ZeroSigma.Application.Authentication.Services.ProcessingServices.SignUpProcessingServices;
using ZeroSigma.Application.Authentication.Services.ValidationServices.Login;
using ZeroSigma.Application.Authentication.Services.ValidationServices.SignUp;
using ZeroSigma.Application.Common.Authentication;

namespace ZeroSigma.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddScoped<ILoginValidationService, LoginValidationService>();
            services.AddScoped<ISignUpValidationService, SignUpValidationService>();   
            services.AddScoped<ISignUpProcessingService,SignUpProcessingService>();
            services.AddScoped<ILoginProcessingService, LoginProcessingService>();
            services.AddScoped<INewSessionProcessingService, NewSessionProcessingService>();
            return services;
        }
    }
}
