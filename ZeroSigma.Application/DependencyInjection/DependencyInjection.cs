﻿using Microsoft.Extensions.DependencyInjection;

namespace ZeroSigma.Application.DependencyInjection
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            return services;
        }
    }
}
