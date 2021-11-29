using System;
using System.Reflection;
using MBD.Core.Data;
using MBD.Core.Identity;
using MBD.CreditCards.API.Configuration.HttpClient;
using MBD.CreditCards.Application.BackgroundServices;
using MBD.CreditCards.Application.IntegrationEvents.Events;
using MBD.CreditCards.Application.IntegrationEvents.Handlers;
using MBD.CreditCards.Application.Interfaces;
using MBD.CreditCards.Application.Services;
using MBD.CreditCards.Domain.Interfaces.Repositories;
using MBD.CreditCards.Domain.Interfaces.Services;
using MBD.CreditCards.Infrastructure;
using MBD.CreditCards.Infrastructure.Repositories;
using MBD.CreditCards.Infrastructure.Services;
using MBD.MessageBus;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;

namespace MBD.CreditCards.API.Configuration
{
    public static class DependencyInjectionConfiguration
    {
        public static IServiceCollection AddDependencyInjection(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAppServices()
                    .AddRepositories()
                    .AddHttpClients(configuration)
                    .AddConsumers()
                    .AddMessageBus()
                    .AddConfigurations(configuration)
                    .AddIntegrationEvents();

            services.AddHttpContextAccessor();
            services.AddScoped<IAspNetUser, AspNetUser>();
            services.AddMediatR(Assembly.GetExecutingAssembly());
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
            services.AddScoped<IBankAccountRepository, BankAccountRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }

        private static IServiceCollection AddHttpClients(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<HttpClientAuthorizationDelegatingHandler>();

            services.AddHttpClient<IBankAccountService, BankAccountService>(config =>
            {
                config.BaseAddress = new Uri(configuration["UrlServices:BankAccountService"]);
            })
            .AddHttpMessageHandler<HttpClientAuthorizationDelegatingHandler>()
            .AddPolicyHandler(PollyRetryConfiguration.WaitToRetry())
            .AddTransientHttpErrorPolicy(p => p.CircuitBreakerAsync(5, TimeSpan.FromSeconds(30)));

            return services;
        }

        private static IServiceCollection AddConsumers(this IServiceCollection services)
        {
            services.AddHostedService<TransactionsConsumerService>();

            return services;
        }

        private static IServiceCollection AddMessageBus(this IServiceCollection services)
        {
            services.AddSingleton<IMessageBus, MessageBus.MessageBus>();

            return services;
        }

        private static IServiceCollection AddConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<RabbitMqConfiguration>(configuration.GetSection(nameof(RabbitMqConfiguration)));

            return services;
        }

        private static IServiceCollection AddIntegrationEvents(this IServiceCollection services)
        {
            services.AddScoped<INotificationHandler<TransactionLinkedToCreditCardBillIntegrationEvent>, TransactionLinkedToCreditCardBillIntegrationEventHandler>();

            return services;
        }
    }
}