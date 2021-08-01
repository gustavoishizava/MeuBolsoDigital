using MBD.Core.Data;
using MBD.Identity.Application.Interfaces;
using MBD.Identity.Application.Services;
using MBD.Identity.Domain.Interfaces.Repositories;
using MBD.Identity.Domain.Interfaces.Services;
using MBD.Identity.Domain.Services;
using MBD.Identity.Infrastructure;
using MBD.Identity.Infrastructure.Repositories;
using MBD.Identity.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace MBD.Identity.API.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddAppServices()
                    .AddDomainServices()
                    .AddInfrastructureServices()
                    .AddRepositories();

            return services;
        }

        private static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationAppService, AuthenticationAppService>()
                    .AddScoped<IUserAppService, UserAppService>();

            return services;
        }

        private static IServiceCollection AddDomainServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticationService, AuthenticationService>()
                    .AddScoped<ICreateUserService, CreateUserService>();

            return services;
        }

        private static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
            services.AddScoped<IHashService, HashService>()
                    .AddScoped<IJwtService, JwtService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}