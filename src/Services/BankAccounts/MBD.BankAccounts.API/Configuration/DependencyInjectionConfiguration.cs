using System.Reflection;
using MBD.BankAccounts.Application.Interfaces;
using MBD.BankAccounts.Application.Services;
using MBD.BankAccounts.Domain.Interfaces.Repositories;
using MBD.BankAccounts.Infrastructure.Repositories;
using MBD.Core.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace MBD.BankAccounts.API.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddAppServices()
                    .AddRepositories();

            services.AddHttpContextAccessor();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddAutoMapper(Assembly.Load("MBD.BankAccounts.Application"));

            return services;
        }

        private static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountAppService, AccountAppService>();

            return services;
        }
       
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccountRepository, AccountRepository>();

            return services;
        }
    }
}