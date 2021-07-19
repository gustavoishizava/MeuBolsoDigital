using System;
using System.Reflection;
using MBD.Core.Identity;
using MBD.CreditCards.API.Configuration.HttpClient;
using MBD.CreditCards.Application.Interfaces;
using MBD.CreditCards.Application.Services;
using MBD.CreditCards.Domain.Interfaces.Repositories;
using MBD.CreditCards.Domain.Interfaces.Services;
using MBD.CreditCards.Infrastructure.Repositories;
using MBD.CreditCards.Infrastructure.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MBD.CreditCards.API.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAppServices()
                    .AddRepositories()
                    .AddHttpClients(configuration);

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

        private static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            var teste = configuration["UrlServices:BankAccountService"];
            services.AddHttpClient<IBankAccountService, BankAccountService>(config =>
            {
                config.BaseAddress = new Uri(teste);
            }).AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>();

            return services;
        }
    }
}