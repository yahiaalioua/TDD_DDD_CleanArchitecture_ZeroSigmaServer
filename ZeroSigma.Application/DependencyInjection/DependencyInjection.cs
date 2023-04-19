﻿using Microsoft.Extensions.DependencyInjection;
using ZeroSigma.Application.Authentication.Services.ValidationServices.Login;
using ZeroSigma.Application.Common.Authentication;

namespace ZeroSigma.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
            services.AddScoped<ILoginValidationService, LoginValidationService>();
            return services;
        }
    }
}
