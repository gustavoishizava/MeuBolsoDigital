using Microsoft.Extensions.DependencyInjection;
using MBD.Core.Identity;
using System.Reflection;
using MBD.CreditCards.Application.Interfaces;
using MBD.CreditCards.Application.Services;
using MBD.CreditCards.Domain.Interfaces.Repositories;
using MBD.CreditCards.Infrastructure.Repositories;

namespace MBD.CreditCards.API.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services)
        {
            services.AddAppServices()
                    .AddRepositories();

            services.AddHttpContextAccessor();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddAutoMapper(Assembly.Load("MBD.CreditCards.Application"));

            return services;
        }

        private static IServiceCollection AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<ICreditCardAppService, CreditCardAppService>();

            return services;
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<ICreditCardRepository, CreditCardRepository>();

            return services;
        }
    }
}